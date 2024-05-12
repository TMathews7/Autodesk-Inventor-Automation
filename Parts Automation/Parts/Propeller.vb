Imports System
Imports System.Net
Imports System.Runtime.InteropServices
Imports Inventor

Namespace PropellerDrawing
    Public Class Propeller
        Public Shared Sub CreatePropellerBase(radius As Double, height As Double, HoleRadius As Double, Wingspan As Double, numBlades As Double)
            Try
                ' Connect to the running instance of Inventor
                Dim inventorApp As Inventor.Application = MarshalHelper.GetActiveObject("Inventor.Application")

                If inventorApp Is Nothing Then
                    Console.WriteLine("Failed to connect to the running instance of Inventor.")
                    Return
                End If

                ' Define the constant for the part document type
                Const kPartDocumentObject As DocumentTypeEnum = DocumentTypeEnum.kPartDocumentObject

                ' Create a new part document using the default part template.
                Dim oPartDoc As PartDocument
                oPartDoc = inventorApp.Documents.Add(kPartDocumentObject, inventorApp.FileManager.GetTemplateFile(kPartDocumentObject))

                ' Set a reference to the part component definition.
                Dim oCompDef As PartComponentDefinition
                oCompDef = oPartDoc.ComponentDefinition

                ' Create a new sketch on the X-Y work plane for the cylinder.
                Dim oSketch1 As PlanarSketch
                oSketch1 = oCompDef.Sketches.Add(oCompDef.WorkPlanes.Item(3))

                ' Set a reference to the transient geometry object.
                Dim oTransGeom As TransientGeometry
                oTransGeom = inventorApp.TransientGeometry

                ' Draw a circle on the sketch for the cylinder.
                Dim oCircle As SketchCircle
                oCircle = oSketch1.SketchCircles.AddByCenterRadius(oTransGeom.CreatePoint2d(0, 0), radius)

                ' Create a profile for the cylinder.
                Dim oProfile As Profile
                oProfile = oSketch1.Profiles.AddForSolid

                ' Create a solid extrusion for the cylinder.
                Dim oExtrudeDef As ExtrudeDefinition
                oExtrudeDef = oCompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(oProfile, PartFeatureOperationEnum.kNewBodyOperation)
                oExtrudeDef.SetDistanceExtent(height, PartFeatureExtentDirectionEnum.kPositiveExtentDirection)
                Dim oExtrusion As ExtrudeFeature
                oExtrusion = oCompDef.Features.ExtrudeFeatures.Add(oExtrudeDef)

                ' Get the center point of the circle
                Dim centerPoint As Point2d = oCircle.CenterSketchPoint.Geometry

                ' Create a new sketch on the same work plane as the circle for the clearance hole.
                Dim oSketch2 As PlanarSketch
                oSketch2 = oCompDef.Sketches.Add(oCompDef.WorkPlanes.Item(3))

                ' Draw a circle on the sketch for the clearance hole.
                Dim oClearanceCircle As SketchCircle
                oClearanceCircle = oSketch2.SketchCircles.AddByCenterRadius(centerPoint, HoleRadius)

                ' Create a profile for the clearance hole.
                Dim oClearanceProfile As Profile
                oClearanceProfile = oSketch2.Profiles.AddForSolid

                ' Create a solid extrusion for the clearance hole.
                Dim oClearanceExtrudeDef As ExtrudeDefinition
                oClearanceExtrudeDef = oCompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(oClearanceProfile, PartFeatureOperationEnum.kCutOperation)
                oClearanceExtrudeDef.SetDistanceExtent(height + 1, PartFeatureExtentDirectionEnum.kPositiveExtentDirection) ' Adds 1 to ensure it goes all the way through,
                Dim oClearanceExtrusion As ExtrudeFeature
                oClearanceExtrusion = oCompDef.Features.ExtrudeFeatures.Add(oClearanceExtrudeDef)

                ' Calculate the angle between each blade
                Dim angleBetweenBlades As Double = 360.0 / numBlades

                Console.WriteLine("Propeller with clearance hole and wings created successfully.")
            Catch ex As Exception
                Console.WriteLine("An error occurred: " & ex.Message)
            End Try
        End Sub
    End Class
End Namespace

' Having serious trouble with getting a circular pattern to function so this is being put on hold until i improve.

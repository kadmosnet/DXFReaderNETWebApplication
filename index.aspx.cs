using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DXFReaderNET;
using DXFReaderNET.Entities;
using System.Drawing;
namespace DXFReaderNETWebApplication
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LiteralButtonUpload.Text = "Upload DXF file...";
            

            if (FileUpload1.HasFile)
            {
                
                string filePath = Server.MapPath("~/Uploads/") + System.IO.Path.GetFileName(FileUpload1.FileName);
                FileUpload1.SaveAs(filePath);
               
                DXFReaderNETControl myDXF = new DXFReaderNETControl();
                if (myDXF.ReadDXF(filePath))
                {
                    myDXF.DXF.DrawingVariables.LwDisplay = false;
                    myDXF.AntiAlias = true;
                    myDXF.ShowAxes = false;
                    myDXF.ShowGrid = false;
                    myDXF.BackColor = Color.White;
                    myDXF.ForeColor = Color.Black;
                    myDXF.Width = 1500;
                    myDXF.Height = 1500;
                    myDXF.DXF.RemoveEntities(myDXF.DXF.Dimensions);

                    foreach (EntityObject ent in myDXF.DXF.Entities)
                    {
                        ent.Color = AciColor.FromCadIndex(7);
                        ent.Lineweight = Lineweight.W0;
                    }

                    myDXF.DisplayPredefinedView(PredefinedViewType.Top);

                    myDXF.ZoomCenter();

                    myDXF.Name = "dxf1";
                    myDXF.Error += DXFErrorEventHandler;
                  

                    {
                        int InternalCountoursNumber = 0;
                        double ExternalLenght = 0d;
                        double ExternalArea = 0d;
                        double InternalLenght = 0d;
                        double InternalArea = 0d;
                        bool ret = MathHelper.FindClosedAreaData(myDXF.DXF.Entities, out ExternalLenght, out ExternalArea, out InternalLenght, out InternalArea, out InternalCountoursNumber);
                       
                        if (ret)
                        {

                            var allEntities = new List<EntityObject>();
                            foreach (EntityObject entity in myDXF.DXF.Entities)
                            {

                                if (entity.Type == EntityType.Line | entity.Type == EntityType.Arc | entity.Type == EntityType.Circle | entity.Type == EntityType.LightWeightPolyline | entity.Type == EntityType.Polyline | entity.Type == EntityType.Ellipse | entity.Type == EntityType.Spline)
                                {

                                    allEntities.Add(entity);
                                }
                            }
                           

                            var ExtMin = new Vector3();
                            var ExtMax = new Vector3();
                            MathHelper.EntitiesExtensions(allEntities, out ExtMin, out ExtMax);
                            Vector2 Extension = new Vector2(ExtMax.X - ExtMin.X, ExtMax.Y - ExtMin.Y);

                           
                            List<EntityObject> entities = myDXF.DXF.Entities.ToList();

                            List<EntityObject> Newentities = new List<EntityObject>();
                            foreach (EntityObject i in entities)
                            {
                                if (i.Type == EntityType.Insert)
                                {
                                    List<EntityObject> insertEntities = ((Insert)i).Explode();
                                    Newentities.AddRange(insertEntities);
                                }


                            }
                            entities.AddRange(Newentities);
                            myDXF.NormalizeEntities(entities);
                            List<EntityObject> entitesToDelete = new List<EntityObject>();


                            if (!myDXF.ShowFilledAreas(allEntities,50,2,5))
                            {
                                LabelError.Text = "External contour not found";
                            }

                            myDXF.Image.Save(Server.MapPath("~/Uploads/") + "\\drawing.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            LiteraImage.Text = "<img src='Uploads/drawing.jpg' width=400>";

                            string colorTab = "393EFF";
                            LiteralText.Text = "<table>";
                         
                            LiteralText.Text += "<tr>";

                            LiteralText.Text += "<td width=350>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + "Drawing extension";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "<td align=right>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + Extension.X.ToString("###,###,###,###,###,###,##0") + " x " + Extension.Y.ToString("###,###,###,###,###,###,##0");
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "</tr>";


                            LiteralText.Text += "<td width=350>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + "Total area";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "<td align=right>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + ExternalArea.ToString("###,###,###,###,###,###,##0");
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "</tr>";


                            LiteralText.Text += "<tr>";
                            LiteralText.Text += "<td>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + "Internal contours number" + "</span>";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "<td align=right>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + InternalCountoursNumber.ToString("###,###,###,###,###,###,##0") + "</span>";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "</tr>";


                            LiteralText.Text += "<tr>";
                            LiteralText.Text += "<td>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + "Empty area" + "</span>";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "<td align=right>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + InternalArea.ToString("###,###,###,###,###,###,##0") + "</span>";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "</tr>";


                            LiteralText.Text += "<tr>";
                            LiteralText.Text += "<td>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + "Effective area" + "</span>";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "<td align=right>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + (ExternalArea- InternalArea).ToString("###,###,###,###,###,###,##0") + "</span>";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "</tr>";
                            LiteralText.Text += "</span>";

                            LiteralText.Text += "<tr>";
                            LiteralText.Text += "<td>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + "External cut length" + "</span>";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "<td align=right>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + ExternalLenght.ToString("###,###,###,###,###,###,##0") + "</span>";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "</tr>";

                            LiteralText.Text += "<tr>";
                            LiteralText.Text += "<td>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + "Internal cut length" + "</span>";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "<td align=right>";
                            LiteralText.Text += "<span style='color:#" + colorTab + ";'>" + InternalLenght.ToString("###,###,###,###,###,###,##0") + "</span>";
                            LiteralText.Text += "</td>";
                            LiteralText.Text += "</tr>";

                            
                            LiteralText.Text += "</table>";

                           

                        }
                    }

                }
            }
        }

        private void DXFErrorEventHandler(object sender, DXFReaderNET.ErrorEventArgs e)
        {

            if (e.ErrorCode > 0)
            {
                LabelError.Text = "Error: " + e.ErrorCode.ToString() + " - " + e.ErrorString;
            }
        }

    }
}
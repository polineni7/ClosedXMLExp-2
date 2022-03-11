using ClosedXML.Excel;
using ClosedXMLExp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace ClosedXMLExp.Controllers
{
    public class MemoryStreamMailExportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public void ExportMain()
        {

            var obj = GetEmails();
            foreach (email a in obj)
            {
                WriteDataToExcel(a);
            }

        }


        public List<email> GetEmails()
        {
            return new List<email>()
            {
                new email(){ Id = "#123", eid="imrampolineni@gmail.com", ename="Ramu", valid = true},
                new email(){ Id = "#321", eid="ramakrishna.polineni@bitapps.com",ename="siva", valid = false},
                new email(){ Id = "#123", eid="ramakrishna.polineni@bitapps.com",ename="Kiran", valid = true},
                new email(){ Id = "#123", eid="ramakrishna.polineni@bitapps.com",ename="siva", valid = true},
                new email(){ Id = "#123", eid="akhil.kotte@bitapps.com",ename="Akhil"},
                new email(){ Id = "1", eid="matean.syed@bitapps.com",ename="Matean"},
                new email(){ Id = "PO_300000005293834_US402429_0", eid="ram.krishna.polineni@gmail.com",ename="Kiran", valid = true},
                new email(){ Id = "PO_300000005293834_US402429_0", eid="ram.krishna.polineni@gmail.com",ename="Polineni", valid = false}
            };
        }

        public List<ClientExport> exportlines([FromBody] string Id)
        {
            dynamic data;
            //List<ClientExport> data = null;
            OMS_DatabaseContext db = new OMS_DatabaseContext();
            {
                data = (from OH in db.OrdHdrs
                        join OD in db.OrdDivs on OH.OrdId equals OD.OrdId
                        join S in db.Statuses on OD.StatusOr equals S.StatusCode
                        where OH.CompanyPo == Id
                        select new ClientExport
                        {
                            CompanyPO = OH.CompanyPo,
                            OrdID = OD.OrdId,
                            OrdName = OD.Descr,
                            OrdDivID = OD.OrdDivId,
                            InqDate = OD.O03date,
                            ReqDate = OD.O01date,
                            QuoteDate = OD.O04date,
                            OrderDate = OD.O05date,
                            ShipDate = OD.O07date,
                            estShipDate = OD.O17date,
                            ArrvDate = OD.O08date,
                            estArrvDate = OD.O18date,
                            InvDate = OD.O09date,
                            PaymentDate = OD.O11date,
                            ClosedDate = OD.O12date,
                            CancelDate = OD.O00date,
                            DelivDate = OD.Delivered,
                            estDelivDate = OD.EstDelivDate,
                            Processed = OD.Processed,
                            Dispatched = OD.Dispached,
                            Fulfilled = OD.O0fdate,
                            Packed = OD.O15date,
                            Picked = OD.O13date,
                            Confirmed = OD.O0cdate,
                            Consolidated = OD.O14date,
                            QtdShipDate = OD.QuotedShipDate,
                            QtdArrDate = OD.QuotedArrivalDate,
                            PromisDelivDate = OD.PromDelivDate,
                            ProgramID = OH.ProgramId,
                            StatusOR = OD.StatusOr,
                            Status = S.Status1,
                            Descr = S.Descr,
                            Client = OH.Company,
                            Incoterms = OD.TotalType,
                            DestinationCity = OD.DestinationCity,
                            DestinationInfo = OD.TotalTypeTo,
                            PaymentTerms = OD.FtPaymentTerms,
                            OrdTotal = OD.OrdTotal,
                            OrdExTotal = OD.OrdExTotal,
                            OrdFxTotal = OD.OrdFtTotal
                        }).OrderByDescending(o => o.ReqDate).ToList();
            }

            return data;
        }
        // GET: Home  
        public void WriteDataToExcel(email x)
        {
            var ClReport = exportlines(x.Id);
            //Name of File  
            //string fileName = "Report.xlsx";

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (XLWorkbook wb = new XLWorkbook())
            {

                var workSheet = wb.Worksheets.Add("Report_Sheet");
                var rowFromRange = workSheet.Range("1:40").FirstRow();
                rowFromRange.Cells("1:40").Style.Fill.BackgroundColor = XLColor.Silver;
                for (int i = 1; i <= 40; i++)
                {
                    workSheet.Cell(1, i).Style.Border.TopBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                    workSheet.Cell(1, i).Style.Border.BottomBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                    workSheet.Cell(1, i).Style.Border.LeftBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                    workSheet.Cell(1, i).Style.Border.RightBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                }
                // rowFromRange.Cells("1:40").Style.Alignment.JustifyLastLine = true;
                rowFromRange.Cells("1:40").Style.Font.Bold = true;
                //rowFromRange.Cells("1:40").Value = "Vertical = Center";
                rowFromRange.Cells("1:40").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                workSheet.Row(1).Height = 20;
                rowFromRange.Cells("1:40").Style.Alignment.WrapText = true;

                workSheet.Columns().AdjustToContents();
                workSheet.Columns("1:40").Width = 18;


                workSheet.Cell(1, 1).Value = "Company PO";
                workSheet.Cell(1, 2).Value = "Order ID";
                workSheet.Cell(1, 3).Value = "Order Name";
                workSheet.Cell(1, 4).Value = "Order Div ID";
                workSheet.Cell(1, 5).Value = "Inquiry Date";
                workSheet.Cell(1, 6).Value = "Requisite Date";
                workSheet.Cell(1, 7).Value = "Quote Date";
                workSheet.Cell(1, 8).Value = "Order Date";
                workSheet.Cell(1, 9).Value = "Ship Date";
                workSheet.Cell(1, 10).Value = "Estimated Ship Date";
                workSheet.Cell(1, 11).Value = "Arriving Date";
                workSheet.Cell(1, 12).Value = "Estimate Arriving Date";
                workSheet.Cell(1, 13).Value = "Inquiry Date";
                workSheet.Cell(1, 14).Value = "Payment Date";
                workSheet.Cell(1, 15).Value = "Closed Date";
                workSheet.Cell(1, 16).Value = "Cancel Date";
                workSheet.Cell(1, 17).Value = "Delivery Date";
                workSheet.Cell(1, 18).Value = "estDelivDate";
                workSheet.Cell(1, 19).Value = "Processed";
                workSheet.Cell(1, 20).Value = "Dispatched";
                workSheet.Cell(1, 21).Value = "Fulfilled";
                workSheet.Cell(1, 22).Value = "Packed";
                workSheet.Cell(1, 23).Value = "Picked";
                workSheet.Cell(1, 24).Value = "Confirmed";
                workSheet.Cell(1, 25).Value = "Consolidated";
                workSheet.Cell(1, 26).Value = "QtdShipDate";
                workSheet.Cell(1, 27).Value = "QtdArrDate";
                workSheet.Cell(1, 28).Value = "PromisDelivDate";
                workSheet.Cell(1, 29).Value = "Program_ID";
                workSheet.Cell(1, 30).Value = "StatusOR";
                workSheet.Cell(1, 31).Value = "Status";
                workSheet.Cell(1, 32).Value = "Description";
                workSheet.Cell(1, 33).Value = "Client";
                workSheet.Cell(1, 34).Value = "Incoterms";
                workSheet.Cell(1, 35).Value = "DestinationCity";
                workSheet.Cell(1, 36).Value = "Destination Info";
                workSheet.Cell(1, 37).Value = "Payment Terms";
                workSheet.Cell(1, 38).Value = "OrdTotal";
                workSheet.Cell(1, 39).Value = "OrdExTotal";
                workSheet.Cell(1, 40).Value = "OrdFxTotal";


                var row = 1;
                foreach (var ci in ClReport)
                {

                    row++;
                    workSheet.Cell(row, 1).Value = ci.CompanyPO;
                    workSheet.Cell(row, 2).Value = ci.OrdID;
                    workSheet.Cell(row, 3).Value = ci.OrdName;
                    workSheet.Cell(row, 4).Value = ci.OrdDivID;
                    workSheet.Cell(row, 5).Value = ci.InqDate;
                    workSheet.Cell(row, 6).Value = ci.ReqDate;
                    workSheet.Cell(row, 7).Value = ci.QuoteDate;
                    workSheet.Cell(row, 8).Value = ci.OrderDate;
                    workSheet.Cell(row, 9).Value = ci.ShipDate;
                    workSheet.Cell(row, 10).Value = ci.estShipDate;
                    workSheet.Cell(row, 11).Value = ci.ArrvDate;
                    workSheet.Cell(row, 12).Value = ci.estArrvDate;
                    workSheet.Cell(row, 13).Value = ci.InvDate;
                    workSheet.Cell(row, 14).Value = ci.PaymentDate;
                    workSheet.Cell(row, 15).Value = ci.ClosedDate;
                    workSheet.Cell(row, 16).Value = ci.CancelDate;
                    workSheet.Cell(row, 17).Value = ci.DelivDate;
                    workSheet.Cell(row, 18).Value = ci.estDelivDate;
                    workSheet.Cell(row, 19).Value = ci.Processed;
                    workSheet.Cell(row, 20).Value = ci.Dispatched;
                    workSheet.Cell(row, 21).Value = ci.Fulfilled;
                    workSheet.Cell(row, 22).Value = ci.Packed;
                    workSheet.Cell(row, 23).Value = ci.Picked;
                    workSheet.Cell(row, 24).Value = ci.Confirmed;
                    workSheet.Cell(row, 25).Value = ci.Consolidated;
                    workSheet.Cell(row, 26).Value = ci.QtdShipDate;
                    workSheet.Cell(row, 27).Value = ci.QtdArrDate;
                    workSheet.Cell(row, 28).Value = ci.PromisDelivDate;
                    workSheet.Cell(row, 29).Value = ci.ProgramID;
                    workSheet.Cell(row, 30).Value = ci.StatusOR;
                    workSheet.Cell(row, 31).Value = ci.Status;
                    workSheet.Cell(row, 32).Value = ci.Descr;
                    workSheet.Cell(row, 33).Value = ci.Client;
                    workSheet.Cell(row, 34).Value = ci.Incoterms;
                    workSheet.Cell(row, 35).Value = ci.DestinationCity;
                    workSheet.Cell(row, 36).Value = ci.DestinationInfo;
                    workSheet.Cell(row, 37).Value = ci.PaymentTerms;
                    workSheet.Cell(row, 38).Value = ci.OrdTotal;
                    workSheet.Cell(row, 39).Value = ci.OrdExTotal;
                    workSheet.Cell(row, 40).Value = ci.OrdFxTotal;
                    for (int i = 1; i <= 40; i++)
                    {
                        workSheet.Cell(row, i).Style.Border.TopBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                        workSheet.Cell(row, i).Style.Border.BottomBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                        workSheet.Cell(row, i).Style.Border.LeftBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                        workSheet.Cell(row, i).Style.Border.RightBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                    }
                }

                wb.SaveAs(stream);
                //Return xlsx Excel File  
                // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);


                writer.Flush();
                stream.Position = 0;     // read from the start of what was written

                /*String ema_pic = "https://www.reliablesupplychains.com/templates/images/rsc-logo.png";*/
                String ema_pwd = "0rd3er$4US!";
                string fromaddr = "Ticketing@reliablesupplychains.com";

                //string toaddr = "imrampolineni@gmail.com";
                MailMessage msg = new MailMessage();
                msg.Subject = "Username &password";
                msg.From = new MailAddress(fromaddr);

                msg.IsBodyHtml = true;

                msg.To.Add(new MailAddress(x.eid));
                string htmlString = "<img src=\"https://www.reliablesupplychains.com/templates/images/rsc-logo.png\" max - width: 100 %; height: auto;>";
                htmlString = htmlString + "<p>Dear Mr/ms." + x.ename + ",</p>";
                htmlString = htmlString + "<p> This is Testing a Report Based on CompanyPO Id.";
                htmlString = htmlString + " <p>Sincerely,<br>-BitApps</br></p>";

                msg.Body = htmlString;
                msg.Attachments.Add(new Attachment(stream, x.ename + ".xlsx"));
                /* msg.IsBodyHtml = true;*/
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.office365.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                NetworkCredential nc = new NetworkCredential(fromaddr, ema_pwd);
                smtp.Credentials = nc;
                smtp.Send(msg);
                stream.Dispose();
                stream.Close();
            }
        }
    }
}

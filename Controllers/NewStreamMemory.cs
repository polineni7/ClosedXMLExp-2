﻿using ClosedXML.Excel;
using ClosedXMLExp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ClosedXMLExp.Controllers
{
    public class NewStreamMemory : Controller
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
                ExportMail(a.Id,a.eid);
            }

        }

        public List<email> GetEmails()
        {
            return new List<email>() {
                new email(){ Id = "#123", eid="imrampolineni@gmail.com"},
                new email(){ Id = "#321", eid="ramakrishna.polineni@bitapps.com"},
                new email(){ Id = "PO_300000005293834_US402429_0", eid="ram.krishna.polineni@gmail.com"}
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

        public void ExportMail(string Id,string toaddr)
        {
            var ClReport = exportlines(Id);
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))    // using UTF-8 encoding by default
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Report Sheet");
                workSheet.Cell(1, 1).Value = "Company PO";
                workSheet.Cell(1, 2).Value = "Order ID";
                workSheet.Cell(1, 3).Value = "Order Name";
                workSheet.Cell(1, 4).Value = "Order Div ID";
                workSheet.Cell(1, 5).Value = "Inq Date";
                workSheet.Cell(1, 6).Value = "Req Date";
                workSheet.Cell(1, 7).Value = "Quote Date";
                workSheet.Cell(1, 8).Value = "Order Date";
                workSheet.Cell(1, 9).Value = "Ship Date";
                workSheet.Cell(1, 10).Value = "estShip Date";
                workSheet.Cell(1, 11).Value = "Arrv Date";
                workSheet.Cell(1, 12).Value = "estArrv Date";
                workSheet.Cell(1, 13).Value = "Inv Date";
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
                workSheet.Cell(1, 29).Value = "ProgramID";
                workSheet.Cell(1, 30).Value = "StatusOR";
                workSheet.Cell(1, 31).Value = "Status";
                workSheet.Cell(1, 32).Value = "Descr";
                workSheet.Cell(1, 33).Value = "Client";
                workSheet.Cell(1, 34).Value = "Incoterms";
                workSheet.Cell(1, 35).Value = "DestinationCity";
                workSheet.Cell(1, 36).Value = "Destination Info";
                workSheet.Cell(1, 37).Value = "Payment Terms";
                workSheet.Cell(1, 38).Value = "OrdTotal";
                workSheet.Cell(1, 39).Value = "OrdExTotal";
                workSheet.Cell(1, 40).Value = "OrdFxTotal";
                workSheet.Columns().AdjustToContents();
                workSheet.Columns("1:40").Width = 15;

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
                }

                wb.SaveAs(stream);

               // writer.WriteLine("Inq Date,Order Date,Order ID,val4");
                writer.Flush();
                stream.Position = 0;     // read from the start of what was written

                String ema_pic = "https://www.reliablesupplychains.com/templates/images/rsc-logo.png", ema_pwd = "0rd3er$4US!";
                string fromaddr = "Ticketing@reliablesupplychains.com";

                //string toaddr = "imrampolineni@gmail.com";
                MailMessage msg = new MailMessage();
                msg.Subject = "Username &password";
                msg.From = new MailAddress(fromaddr);
                msg.To.Add(new MailAddress(toaddr));
                msg.Body = "GridView Exported Excel Attachment  *This is an automatically generated email, please do not reply*";
                msg.Body = ema_pic;
                msg.Attachments.Add(new Attachment(stream, "filename.csv", "text/csv"));
                msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.office365.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                NetworkCredential nc = new NetworkCredential(fromaddr, ema_pwd);
                smtp.Credentials = nc;
                smtp.Send(msg);
            }
        }
    }
}

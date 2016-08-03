using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;

namespace _CertifiedLabel.Controllers
{
    public class CertifiedLabelController : ApiController
    {
        private CityViewEntities db = new CityViewEntities();
        private string url = @"C:\CertifiedLabel\";
        private string Remove_Special_Char(string str)
        {
            if (str == null)
            {
                return str;
            }
            else
            {
                str.Replace('#', '*');
                str.Replace('&', '$');
                str.Trim();
                return str;
            }
        }
        private string GetRequest(int RecordID)
        {
            string addressContact = db.CELetterRecipients.Where(x => x.RecordID == RecordID).FirstOrDefault().Name;
            string AddresseeCompany = db.CELetterRecipients.Where(x => x.RecordID == RecordID).FirstOrDefault().BusinessName;
            string AddresseeAddress = db.CELetterRecipients.Where(x => x.RecordID == RecordID).FirstOrDefault().Address1;
            string AddresseeAddress2 = db.CELetterRecipients.Where(x => x.RecordID == RecordID).FirstOrDefault().Address2;
            string AddresseeCity = db.CELetterRecipients.Where(x => x.RecordID == RecordID).FirstOrDefault().City;
            string AddresseeZip = db.CELetterRecipients.Where(x => x.RecordID == RecordID).FirstOrDefault().Zip;
            int CELetterID = (int)db.CELetterRecipients.Where(x => x.RecordID == RecordID).FirstOrDefault().CELetterID;
            int CECaseID = (int)db.CELetters.Where(x => x.RecordID == CELetterID).FirstOrDefault().CECaseID;
            string Inspector = db.CECases.Where(x => x.RecordID == CECaseID).FirstOrDefault().Inspector;
            int CESectorID = (int)db.LookupEmployees.Where(x => x.Code == Inspector).FirstOrDefault().CESECTID;
            string SenderCompany = db.LookupCESectorsInfoes.Where(x => x.CESectorID == CESectorID).FirstOrDefault().Desc;
            string SenderAddress = db.LookupCESectorsInfoes.Where(x => x.CESectorID == CESectorID).FirstOrDefault().SectorAddress.Trim();
            string SenderCity = db.LookupCESectorsInfoes.Where(x => x.CESectorID == CESectorID).FirstOrDefault().SectorCity.Trim();
            string SenderState = db.LookupCESectorsInfoes.Where(x => x.CESectorID == CESectorID).FirstOrDefault().SectorState.Trim();
            string SenderZip = db.LookupCESectorsInfoes.Where(x => x.CESectorID == CESectorID).FirstOrDefault().SectorZipCode.Trim();

            string FileNumber = db.CECases.Where(x => x.RecordID == CECaseID).FirstOrDefault().CaseNumber;

            string SenderContact = "";
            if (FileNumber.StartsWith("BB"))
            {
                SenderContact = "Building Department";
            }
            else
            {
                if (FileNumber.StartsWith("SW"))
                {

                    SenderContact = "Solid Waste Department";
                }
                else
                {
                    SenderContact = "Code Compliance Department";
                }
            }


            string urlAddress = "http://www.printcertifiedmail.com/generateCertified.aspx?";

            urlAddress += "UserID=1689";

            urlAddress += "&UserEmail=csalazar@miamigov.com";

            urlAddress += "&AddresseeContact=" + Remove_Special_Char(addressContact);

            urlAddress += "&AddresseeCompany=" + Remove_Special_Char(AddresseeCompany);

            urlAddress += "&AddresseeAddress=" + Remove_Special_Char(AddresseeAddress);

            urlAddress += "&AddresseeAddress2=" + Remove_Special_Char(AddresseeAddress2);

            urlAddress += "&AddresseeCity=" + Remove_Special_Char(AddresseeCity);

            urlAddress += "&AddresseeState=";

            urlAddress += "&AddresseeZip=" + AddresseeZip;

            urlAddress += "&InternalCode=x_internalcode";

            urlAddress += "&FileNumber=" + Remove_Special_Char(FileNumber);

            urlAddress += "&InternalFileNumber=x_internalfilenumber";

            urlAddress += "&SenderContact=" + Remove_Special_Char(SenderContact);

            urlAddress += "&SenderCompany=Company: " + Remove_Special_Char(SenderCompany);

            urlAddress += "&SenderAddress=" + Remove_Special_Char(SenderAddress);

            urlAddress += "&SenderAddress2=";

            urlAddress += "&SenderCity=" + Remove_Special_Char(SenderCity);

            urlAddress += "&SenderState=" + Remove_Special_Char(SenderState);

            urlAddress += "&SenderZip=" + Remove_Special_Char(SenderZip);

            urlAddress += "&Weight=1";

            urlAddress += "&ReturnReceipt=";

            urlAddress += "&ERR=checkbox";

            urlAddress += "&RestrictedDelivery=";

            urlAddress += "&Optlbl=1";

            urlAddress += "&SenderID=1865";

            urlAddress += "&CustomerNumber=";

            urlAddress += "&DUNSNumber=900176001";

            urlAddress += "&ERRDeliveryMethod=FTP";

            urlAddress += "&ERRPaymentMethod=Meter/PCPostage";

            urlAddress += "&PackageType=Letters";

            urlAddress += "&FormType=15";
            return urlAddress;
        }

        private string GetName(string filepath)
        {
            char[] temp = new char[26];
            filepath.CopyTo(9, temp, 0, 26);
            return new string(temp);
        }

        //Function GetRecordID() As String
        //    Try
        //        Dim ad As ApplicationDeployment = ApplicationDeployment.CurrentDeployment
        //        Dim sQueryString As String = ad.ActivationUri.ToString()
        //        sQueryString = sQueryString.Split("?")(1).Split("=")(1)
        //        Return sQueryString
        //    Catch ex As Exception
        //        Return ""
        //    End Try
        //End Function


        //Dim strRecordID As String = GetRecordID()
        //If[String].IsNullOrEmpty(strRecordID) Then
        //    'MsgBox("There is not parameter within the querystring!")
        //    'Close()
        //    'Return
        //    objCert.record_id = "11116" '"11103" '"275197" '"10836"
        //Else
        //    objCert.record_id = strRecordID
        //End If
        //If (objCert.certified_number_notexists()) Then
        //  Try
        //      If objCert.get_querystring_values() Then
        //          Me.txtCaseNumber.Text = objCert.case_number
        //          Me.txtSenderCompany.Text = objCert.sender_company
        //          Me.txtSenderContact.Text = objCert.sender_contact
        //          Me.txtSenderAddress1.Text = objCert.sender_address1
        //          Me.txtSenderAddress2.Text = objCert.sender_address2
        //          Me.txtCity.Text = objCert.sender_city
        //          Me.txtState.Text = objCert.sender_state
        //          Me.txtSenderZipCode.Text = objCert.sender_zip
        //          Me.txtAddresseeContact.Text = objCert.addressee_contact
        //          Me.txtAddresseeCompanyName.Text = objCert.addressee_company
        //          Me.txtAddresseeAddress1.Text = objCert.addressee_address1
        //          Me.txtAddresseeAddress2.Text = objCert.addressee_address2
        //          Me.txtAddresseeState.Text = objCert.addressee_state
        //          Me.txtAddresseeCity.Text = objCert.addressee_city
        //          Me.txtAddresseeZipCOde.Text = objCert.addressee_zip

        //          Dim url As String = objCert.build_url_address()

        //          If Not [String].IsNullOrEmpty(url) Then
        //              WebBrowser1.Navigate(url)
        //              Me.lblMessage.Text = "Loading the Certified Label..."
        //          Else
        //              MsgBox("Cannot navigate to an empty or null string!", MsgBoxStyle.Exclamation)
        //          End If
        //          ProgressBar1.Show()
        //      Else
        //          MsgBox("Unable to query the database!", MsgBoxStyle.Exclamation)
        //      End If
        //  Catch ex As Exception
        //      MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        //  End Try

        //localhost/CertifiedLabel?RecordID=183421  --123322 --159241
        //public string Getname(string name, string lastname)
        //{
        //    return name + " " + lastname;
        //}
        public HttpResponseMessage GetLabel(int RecordID)
        {

            if (RecordID != -1)
            {
                CELetterRecipient celetter_recipient = db.CELetterRecipients.Where(x => x.RecordID == RecordID).FirstOrDefault();
                if (celetter_recipient.CertifiedNumber == null)
                {
                    HttpResponseMessage response = new HttpResponseMessage();

                    string urladdress = GetRequest(RecordID);



                    HttpClient client = new HttpClient();

                    response = client.GetAsync(urladdress).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string filename = GetName(response.RequestMessage.RequestUri.LocalPath);

                        celetter_recipient.CertifiedNumber = filename;
                        db.Entry(celetter_recipient).State = EntityState.Modified;
                        db.SaveChanges();

                        DirectoryInfo folder = new DirectoryInfo(url + filename);
                        folder.Create();


                        FileStream stream = new FileStream(folder.FullName + "\\" + filename + ".pdf", FileMode.CreateNew);
                        response.Content.CopyToAsync(stream);

                        //Document pdf = new Document();
                        //PdfWriter.GetInstance(pdf, new FileStream(url + filename + "\\" + filename, FileMode.Create));
                        //pdf.Open();
                        //response.Content.CopyToAsync();
                        //Document document = new Document();

                        //PdfWriter.GetInstance(document,

                        //              new FileStream("devjoker.pdf",

                        //                     FileMode.OpenOrCreate));

                        //document.Open();

                        //document.Add(new Paragraph("Este es mi primer PDF al vuelo"));

                        //document.Close();
                        return response;
                    }
                    else
                    {
                        return new HttpResponseMessage(response.StatusCode);
                    }
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public HttpResponseMessage GetSignature(string CertifiedNumber)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            string request = @"http://74.205.98.40/USPS_Signatures/900176001/" + CertifiedNumber + ".pdf";
            response = client.GetAsync(request).Result;
            return response;
        }
        public HttpResponseMessage GetAllSignature()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            string request = @"http://74.205.98.40/USPS_Signatures/900176001/";
            response = client.GetAsync(request).Result;
            return response;
        }

        // GET: 
        /* api/CertifiedLabel?UserID=1689&SenderID=1865&UserEmail=csalazar@miamigov.com&DepartmentName=Test2&AplicationName=Test2&Sector=Test&SenderAddress=Test&SenderAddress2=Test&SenderCity=Miami&SenderState=FL&SenderZip=33141&SenderPhone=12345678&CaseNumber=Test123456&SupervisorName=JuanCarlos&Description=Test&Subject=Test&Date=06/26/2016&Contact=Test&Company=Test&Address=Test&Address2=Test&City=Miami&State=FL&Zip=33181
         
         */
        //public HttpResponseMessage Get(
        // int UserID,
        // int SenderID,
        // string UserEmail,
        // string DepartmentName,
        // string AplicationName,
        // string Sector,
        // string SenderAddress,
        // string SenderAddress2,
        // string SenderCity,
        // string SenderState,
        // string SenderZip,
        // string SenderPhone,
        // string CaseNumber,
        // string SupervisorName,
        // string Description,
        // string Subject,
        // string Date,
        // string Contact,
        // string Company,
        // string Address,
        // string Address2,
        // string City,
        // string State,
        // string Zip)
        //{


        //    HttpRequest request = new HttpRequest()
        //    {
        //        UserID = UserID,
        //        SenderID = SenderID,
        //        UserEmail = UserEmail,
        //        DepartmentName = DepartmentName,
        //        AplicationName = AplicationName,
        //        Sector = Sector,
        //        SenderAddress = SenderAddress,
        //        SenderAddress2 = SenderAddress2,
        //        SenderCity = SenderCity,
        //        SenderState = SenderState,
        //        SenderZip = SenderZip,
        //        SenderPhone = SenderPhone,
        //        CaseNumber = CaseNumber,
        //        SupervisorName = SupervisorName,
        //        Description = Description,
        //        Subject = Subject,
        //        Date = Date,
        //        Contact = Contact,
        //        Company = Company,
        //        Address1 = Address,
        //        Address2 = Address2,
        //        City = City,
        //        State = State,
        //        Zip = Zip
        //    };

        //    Department department = new Department()
        //    {
        //        DepartmentName = DepartmentName,
        //        UserID = UserID,
        //        UserEmail = UserEmail,
        //        Sector = Sector,
        //        Address = Address,
        //        Address2 = Address2,
        //        City = City,
        //        State = State,
        //        Zip = Zip,
        //        Phone = SenderPhone
        //    };

        //    Case _case = new Case()
        //    {
        //        CaseNumber = CaseNumber,
        //        SupervisorName = SupervisorName,
        //        Description = Description,
        //        DepartmentName = DepartmentName
        //    };

        //    Letter letter = new Letter()
        //    {
        //        DepartmentName = DepartmentName,
        //        CaseNumber = CaseNumber,
        //        Subject = Subject,
        //        Date = Date,
        //        Contact = Contact,
        //        Company = Company,
        //        Address1 = Address,
        //        Address2 = Address2,
        //        City = City,
        //        State = State,
        //        Zip = Zip
        //    };
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    HttpClient client = new HttpClient();
        //    response = client.GetAsync(GetRequest(request)).Result;

        //    string filename = GetName(response.RequestMessage.RequestUri.LocalPath);
        //    request.Name = filename;
        //    letter.Name = filename;
        //    request.Url = url + filename + ".pdf";
        //    letter.Url = url + filename + ".pdf";
        //    department.Cases.Add(_case);
        //    department.Letters.Add(letter);
        //    _case.Department = department;
        //    _case.Letters.Add(letter);
        //    letter.Department = department;
        //    letter.Case = _case;

        //    db.Cases.Add(_case);
        //    db.Departments.Add(department);
        //    db.Letters.Add(letter);
        //    db.HttpRequests.Add(request);

        //    db.SaveChanges();
        //    return response;
    }
}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using e_DataPrivacy.BayerAuthorizedService;
using System.IO;
using System.Configuration;
using System.Data.Objects;
using System.Data.EntityClient;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace e_DataPrivacy.Model
{
    public class DataPrivacyDataServices
    {
        public static sp_user_role_Result checkRole(string cwid, string app)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    var query = from obj in db.sp_user_role(cwid, app)
                                select obj;
                    return query.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<M_Document_Template> GetTemplate(int FP_ID)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    var obj = from a in db.M_Document_Template
                              where a.Functional_Process_ID == FP_ID
                              select a;
                    return obj.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<sp_GetAllData_Result> GetDataPrivacy()
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    return db.sp_GetAllData().ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool FP01Personal(int id, string name, string filename, string path, string remark,string cwid, bool newPurpose, string caretaker, out int ID)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            T_Personal_Data_Request FP01 = new T_Personal_Data_Request();
                            bool approve = true;
                            bool returned = false;
                            bool rejected = false;
                            FP01.Functional_Process_ID = id;
                            FP01.Name = name;
                            FP01.Caretaker = caretaker;
                            FP01.Document_File_Name = filename;
                            FP01.Document_Path = path;
                            FP01.Remarks = remark;
                            FP01.Approved = approve;
                            FP01.Returned = returned;
                            FP01.Rejected = rejected;
                            FP01.Created_By = cwid;
                            FP01.Created_Date = DateTime.Now;
                            FP01.Is_New_Purpose = newPurpose;
                            FP01.Status = true;

                            db.T_Personal_Data_Request.AddObject(FP01);
                            db.SaveChanges();

                            transaction.Commit();
                            db.Connection.Close();
                            ID = FP01.Personal_Data_Request_ID;

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Create FP01", "Create FP01 with ID : " + ID.ToString() + ", Name : " + name + "by : "+caretaker+ ".", false, false, cwid);

                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            ID = 0;
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Create FP01", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ID = 0;
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Create FP01", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                
                return false;
            }
        }

        public static bool FP01Group(int id, string activity, string filename, string path, string remark, string cwid, bool newPurpose, string caretaker, DateTime date, out int ID)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            T_Personal_Data_Request FP01 = new T_Personal_Data_Request();
                            bool approve = true;
                            bool returned = false;
                            bool rejected = false;
                            FP01.Functional_Process_ID = id;
                            FP01.Activity = activity;
                            FP01.Caretaker = caretaker;
                            FP01.Document_File_Name = filename;
                            FP01.Document_Path = path;
                            FP01.Remarks = remark;
                            FP01.Approved = approve;
                            FP01.Returned = returned;
                            FP01.Rejected = rejected;
                            FP01.Created_By = cwid;
                            FP01.Created_Date = DateTime.Now;
                            FP01.Is_New_Purpose = newPurpose;
                            FP01.Activity_Date = date;
                            FP01.Status = true;
                            //if (app > 0)
                            //{
                            //    FP01.Application = app;
                            //}

                            db.T_Personal_Data_Request.AddObject(FP01);
                            db.SaveChanges();

                            transaction.Commit();
                            db.Connection.Close();
                            ID = FP01.Personal_Data_Request_ID;

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Create FP01", "Create FP01 with ID : " + ID.ToString() + ", Activity : " + activity + "by : " +caretaker+ ".", false, false, cwid);

                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            ID = 0;
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Create FP01", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ID = 0;
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Create FP01", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static bool FP02(int id, string name, string filename, string path, string remark,string cwid, bool approved, bool delete, bool edit,bool withdrawal, string caretaker, out int ID)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            T_Personal_Data_Request FP = new T_Personal_Data_Request();
                            
                            bool rejected = false;
                            bool returned = false;
                            FP.Name = name;
                            FP.Functional_Process_ID = id;
                            FP.Document_File_Name = filename;
                            FP.Document_Path = path;
                            FP.Remarks = remark;
                            FP.Approved = approved;
                            FP.Rejected = rejected;
                            FP.Returned = returned;
                            FP.Created_By = cwid;
                            FP.Created_Date = DateTime.Now;
                            FP.Is_Delete = delete;
                            FP.Is_Modification = edit;
                            FP.Caretaker = caretaker;
                            FP.Status = true;
                            FP.Is_Withdrawal = withdrawal;
                            
                            db.T_Personal_Data_Request.AddObject(FP);
                            db.SaveChanges();

                            transaction.Commit();
                            db.Connection.Close();
                            ID = FP.Personal_Data_Request_ID;

                            if (delete == true)
                            {
                                db.sp_SendFP02DeleteEmail(ID);

                                AuthorizedClient Obj = new AuthorizedClient();
                                Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Create FP02 - Delete", "Create FP02-Change with ID : " + ID.ToString() + ", Name : " + name + ".", false, false, cwid);
                            }
                            else if (withdrawal == true)
                            {
                                db.sp_SendFP02WithdrawalEmail(ID);
                                AuthorizedClient Obj = new AuthorizedClient();
                                Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Create FP02 - Withdrawal", "Create FP02-Change with ID : " + ID.ToString() + ", Name : " + name + ".", false, false, cwid);
                            }
                            else
                            {
                                db.sp_SendFP02ChangeEmail(ID);
                                AuthorizedClient Obj = new AuthorizedClient();
                                Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Create FP02 - Change", "Create FP02-Change with ID : " + ID.ToString() + ", Name : " + name + ".", false, false, cwid);

                            }
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            ID = 0;
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Create FP02", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ID = 0;
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Create FP02", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static List<T_Personal_Data_Request> ListFP01()
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    var obj = from a in db.T_Personal_Data_Request
                              where a.Functional_Process_ID == 1 && a.Approved == true
                              select a;
                    return obj.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool updateApprove(int ID,string remarks,DateTime date,string cwid)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            List<T_Personal_Data_Request> question = db.T_Personal_Data_Request.Where(a => a.Personal_Data_Request_ID == ID).ToList();
                            foreach (T_Personal_Data_Request obj in question)
                            {
                                T_Personal_Data_Request PDR = db.T_Personal_Data_Request.First(x => x.Personal_Data_Request_ID == ID);
                                bool status = true;
                                PDR.Approved = status;
                                PDR.Approved_Remarks = remarks;
                                PDR.Approved_Date = date;
                                PDR.Approved_By = cwid;

                                db.T_Personal_Data_Request.ApplyCurrentValues(PDR);
                                db.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                            }

                            transaction.Commit();
                            db.Connection.Close();

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Approve FP02 - Deletion", "Approve FP02 - Deletion with ID : " + ID + ", Remarks : " + remarks +".", false, false, cwid);
                            sendEmailApprove(ID);

                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Approve FP02 Deletion", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Approve FP02 Deletion", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static bool updateReject(int ID, string remarks, DateTime date, string cwid)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            List<T_Personal_Data_Request> question = db.T_Personal_Data_Request.Where(a => a.Personal_Data_Request_ID == ID).ToList();
                            foreach (T_Personal_Data_Request obj in question)
                            {
                                T_Personal_Data_Request PDR = db.T_Personal_Data_Request.First(x => x.Personal_Data_Request_ID == ID);
                                bool status = true;
                                PDR.Rejected = status;
                                PDR.Rejected_Remarks = remarks;
                                PDR.Rejected_Date = date;
                                PDR.Rejected_By = cwid;

                                db.T_Personal_Data_Request.ApplyCurrentValues(PDR);
                                db.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                            }

                            transaction.Commit();
                            db.Connection.Close();

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Reject FP02 - Deletion", "Reject FP02 - Deletion with ID : " +ID+ ", Remarks : " + remarks + ".", false, false, cwid);
                            sendEmailReject(ID);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Reject FP02 Deletion", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Reject FP02 Deletion", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static bool updateReturned(int ID, string remarks, DateTime date, string cwid)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            List<T_Personal_Data_Request> question = db.T_Personal_Data_Request.Where(a => a.Personal_Data_Request_ID == ID).ToList();
                            foreach (T_Personal_Data_Request obj in question)
                            {
                                T_Personal_Data_Request PDR = db.T_Personal_Data_Request.First(x => x.Personal_Data_Request_ID == ID);
                                bool status = true;
                                PDR.Returned = status;
                                PDR.Returned_Remarks = remarks;
                                PDR.Returned_Date = date;
                                PDR.Returned_By = cwid;

                                db.T_Personal_Data_Request.ApplyCurrentValues(PDR);
                                db.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                            }

                            transaction.Commit();
                            db.Connection.Close();

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Return FP02 - Deletion", "Return FP02 - Deletion with ID : " + ID + ", Remarks : " + remarks + ".", false, false, cwid);
                            sendEmailReturn(ID);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Return FP02 Deletion", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Return FP02 Deletion", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static bool updateFP(int ID, string filename, string absolutePath)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            List<T_Personal_Data_Request> option = db.T_Personal_Data_Request.Where(a => a.Personal_Data_Request_ID == ID).ToList();
                            foreach (T_Personal_Data_Request obj in option)
                            {
                                T_Personal_Data_Request FP = db.T_Personal_Data_Request.First(x => x.Personal_Data_Request_ID == ID);
                                FP.Document_File_Name = filename;
                                FP.Document_Abs_Path = absolutePath;
                                db.T_Personal_Data_Request.ApplyCurrentValues(FP);
                                db.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                            }

                            transaction.Commit();
                            db.Connection.Close();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<sp_GetAllData_Result> FPReturned(string cwid)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    
                    var obj = GetDataPrivacy().Where(x => x.Approved == false && x.Rejected == false && x.Returned == true && x.Created_By == cwid);
                    return obj.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<T_Personal_Data_Request> dataReturn(int id)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    var obj = from a in db.T_Personal_Data_Request
                              where a.Personal_Data_Request_ID==id
                              select a;
                    return obj.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool reSubmit(int ID, string name, string caretaker, string upload, string path, string absolutPath, string remarks,string cwid)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            List<T_Personal_Data_Request> option = db.T_Personal_Data_Request.Where(a => a.Personal_Data_Request_ID == ID).ToList();
                            foreach (T_Personal_Data_Request obj in option)
                            {
                                T_Personal_Data_Request FP = db.T_Personal_Data_Request.First(x => x.Personal_Data_Request_ID == ID);
                                if(!string.IsNullOrWhiteSpace(upload))
                                {
                                    FP.Document_File_Name = upload;
                                    FP.Document_Path = path;
                                    FP.Document_Abs_Path = absolutPath;
                                }
                                FP.Name = name;
                                FP.Caretaker = caretaker;                                
                                FP.Remarks = remarks;
                                FP.Returned = false;
                                FP.Modified_By = cwid;
                                FP.Modified_Date = DateTime.Now;

                                db.T_Personal_Data_Request.ApplyCurrentValues(FP);
                                db.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                            }
                            transaction.Commit();
                            db.Connection.Close();

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Resubmit FP02-Deletion", "Resubmit FP02 with ID : " + ID + ", name" + name +", caretaker : " +caretaker+ ", Document File Name : "+upload+", remarks : "+remarks+".", false, false, cwid);

                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Resubmit FP02-Deletion", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Resubmit FP02-Deletion", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static bool FP10(int id, string name, string incident,string cronology, string cwid,string upload, string path, out int Id)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            T_Personal_Data_Request PDR = new T_Personal_Data_Request();
                            bool approve = true;
                            bool returned = false;
                            bool rejected = false;
                            PDR.Functional_Process_ID = id;
                            PDR.Name = name;
                            PDR.Approved = approve;
                            PDR.Returned = returned;
                            PDR.Rejected = rejected;
                            PDR.Created_By = cwid;
                            PDR.Created_Date = DateTime.Now;
                            PDR.Incident = incident;
                            PDR.Cronology = cronology;
                            PDR.Is_Closed = false;
                            PDR.Status = true;
                            PDR.Document_File_Name = upload;
                            PDR.Document_Path = path;

                            db.T_Personal_Data_Request.AddObject(PDR);
                            db.SaveChanges();

                            transaction.Commit();
                            db.Connection.Close();

                            Id = PDR.Personal_Data_Request_ID;

                           
                                AuthorizedClient Obj = new AuthorizedClient();
                                Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Create FP10", "Create FP10 with ID : " + Id.ToString() + ", Name : " + name + "Incident : " + incident + ".", false, false, cwid);

                                db.sp_SendFP10CreateEmail(Id);

                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            Id = 0;
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Create FP10", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);

                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Id = 0;

                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Create FP10", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid); 

                return false;
            }
        }

        public static bool CloseIncident(int ID, string cwid, string action)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            List<T_Personal_Data_Request> question = db.T_Personal_Data_Request.Where(a => a.Personal_Data_Request_ID == ID).ToList();
                            foreach (T_Personal_Data_Request obj in question)
                            {
                                T_Personal_Data_Request PDR = db.T_Personal_Data_Request.First(x => x.Personal_Data_Request_ID == ID);
                                bool status = true;
                                PDR.Is_Closed = status;
                                PDR.Closed_By = cwid;
                                PDR.Closed_Date = DateTime.Now;
                                PDR.Action = action;

                                db.T_Personal_Data_Request.ApplyCurrentValues(PDR);
                                db.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                            }
                            transaction.Commit();
                            db.Connection.Close();

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Close Incident", "Close Incident with ID : "+ID+ ", action and solution : " +action+ ".", false, false, cwid);

                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Close Incident", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Close Incident", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static bool createEvent(string name, int participant, int division, string cwid, string PIC, DateTime eventdate ,List<sp_question_Result>AnswerDetail, out int ID)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            bool approved = false;
                            bool returned = false;
                            bool rejected = false;
                            T_Event Event = new T_Event();
                            Event.Event_Name = name;
                            Event.PIC = PIC;
                            Event.Event_Date = eventdate;
                            Event.Participant_Class = participant;
                            Event.Division = division;
                            Event.Created_Date = DateTime.Now;
                            Event.Created_By = cwid;
                            Event.Approved = approved;
                            Event.Returned = returned;
                            Event.Rejected = rejected;
                            
                            db.T_Event.AddObject(Event);
                            db.SaveChanges();

                            ID = Event.Event_ID;

                            foreach (sp_question_Result AD in AnswerDetail)
                            {
                                T_Inventory_Answer answer = new T_Inventory_Answer();
                                answer.Event_ID = ID;
                                answer.Inventory_Question_ID = AD.Inventory_Question_ID;
                                answer.Answer = AD.Answer;
                                answer.Question_Order = AD.Inventory_Question_Order;
                                answer.Status = true;
                                answer.Created_By = cwid;
                                answer.Created_Date = DateTime.Now;
                                db.T_Inventory_Answer.AddObject(answer);
                                db.SaveChanges();
                            }

                            transaction.Commit();
                            
                            db.Connection.Close();

                            db.sp_SendEventNeedApprovalEmail(ID);
                            
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Create Event", "Create Event with ID : " +ID, false, false, cwid);
                            
                            return true;
                        }
                        catch(Exception ex) 
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            ID = 0;

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Create Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);

                            return false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ID = 0;
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Create Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);

                return false;
            }
        }

        public static List<vEmployee> getEmployee()
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    var obj = from a in db.vEmployees
                              select a;
                    return obj.ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        
        public static bool approvedEvent(int ID,string remarks,string cwid)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            List<T_Event> ev = db.T_Event.Where(a => a.Event_ID == ID).ToList();
                            foreach (T_Event obj in ev)
                            {
                                T_Event even = db.T_Event.First(x => x.Event_ID == ID);
                                bool approve=true;
                                even.Approved = approve;
                                even.Approved_Remarks = remarks;
                                even.Approved_Date = DateTime.Now;
                                even.Approved_By = cwid;

                                db.T_Event.ApplyCurrentValues(even);
                                db.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                            }

                            transaction.Commit();
                            db.Connection.Close();

                            db.sp_SendEventApproveEmail(ID);

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Approve Event", "Approve Event with ID : " + ID + ", Remarks : " + remarks + ".", false, false, cwid);

                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Approve Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Approve Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static bool rejectEvent(int ID, string remarks, string cwid)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            List<T_Event> ev = db.T_Event.Where(a => a.Event_ID == ID).ToList();
                            foreach (T_Event obj in ev)
                            {
                                T_Event even = db.T_Event.First(x => x.Event_ID == ID);
                                bool status = true;
                                even.Rejected = status;
                                even.Rejected_Remarks = remarks;
                                even.Rejected_Date = DateTime.Now;
                                even.Rejected_By = cwid;

                                db.T_Event.ApplyCurrentValues(even);
                                db.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                            }

                            transaction.Commit();
                            db.Connection.Close();

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Reject Event", "Reject Event with ID : " + ID + ", Remarks : " + remarks + ".", false, false, cwid);

                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Reject Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Reject Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static bool returnEvent(int ID, string remarks, string cwid)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            List<T_Event> question = db.T_Event.Where(a => a.Event_ID == ID).ToList();
                            foreach (T_Event obj in question)
                            {
                                T_Event PDR = db.T_Event.First(x => x.Event_ID == ID);
                                bool status = true;
                                PDR.Returned = status;
                                PDR.Returned_Remarks = remarks;
                                PDR.Returned_Date = DateTime.Now;
                                PDR.Returned_By = cwid;

                                db.T_Event.ApplyCurrentValues(PDR);
                                db.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                            }

                            transaction.Commit();
                            db.Connection.Close();

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Return Event", "Return Event with ID : " + ID + ", Remarks : " + remarks + ".", false, false, cwid);

                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Return Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Return Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static List<T_Event> EventReturned(string cwid)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    var obj = from a in db.T_Event where a.Returned==true && a.Created_By==cwid
                              select a;
                    return obj.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool reSubmit_Event(int ID, string name, string PIC, DateTime date, int participant, int division, List<sp_GetAnswer_Result> AnswerDetail, string cwid)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        try
                        {
                            List<T_Event> events = db.T_Event.Where(a => a.Event_ID == ID).ToList();
                            foreach (T_Event obj in events)
                            {
                                T_Event ev = db.T_Event.First(x => x.Event_ID == ID);
                                
                                ev.Event_Name = name;
                                ev.Division = division;
                                ev.Participant_Class = participant;
                                ev.PIC = PIC;
                                ev.Modified_By = cwid;
                                ev.Modified_Date = DateTime.Now;
                                ev.Event_Date = date;
                                ev.Returned = false;
                                ev.Returned_By = null;
                                ev.Returned_Date = null;
                                ev.Returned_Remarks = null;

                                db.T_Event.ApplyCurrentValues(ev);
                                db.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                                
                                foreach (var AD in AnswerDetail)
                                {
                                    int id=Convert.ToInt32(AD.Inventory_Answer_ID);
                                    List<T_Inventory_Answer> answerlist = db.T_Inventory_Answer.Where(a => a.Inventory_Answer_ID == id).ToList();
                                    foreach (T_Inventory_Answer ans in answerlist)
                                    {
                                        ans.Answer = AD.Answer;
                                        ans.Modified_By = cwid;
                                        ans.Modified_Date = DateTime.Now;
                                        
                                        db.T_Inventory_Answer.ApplyCurrentValues(ans);
                                        db.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                                    }
                                        
                                    
                                }
                            }
                            transaction.Commit();
                            db.Connection.Close();

                            db.sp_SendEventNeedApprovalEmail(ID);

                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveAuditTrail(HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, "Resubmit Event", "Resubmit FP02 with ID : " + ID + ", Event Name" + name + ", Caretaker : " + PIC +   ", Date : " + date + ".", false, false, cwid);

                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            db.Connection.Close();
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Resubmit Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Resubmit Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static List<sp_GetEventData_Result> GetEvent()
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    return db.sp_GetEventData().ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool sendEmailApprove(int id)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.sp_SendFP02ApproveEmail(id);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool sendEmailChange(int id)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.sp_SendFP02ChangeEmail(id);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool sendEmailReturn(int id)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.sp_SendFP02ReturnEmail(id);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool sendEmailDelete(int id)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.sp_SendFP02DeleteEmail(id);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool sendEmailReject(int id)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.sp_SendFP02RejectEmail(id);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static List<sp_question_Result> GetQuestion(int id)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    return db.sp_question(id).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<sp_Division_Result> GetDivision()
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    return db.sp_Division().ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool saveAnswer(int ID, List<sp_question_Result>AnswerDetail, string cwid)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    db.Connection.Open();
                    using (var trans = db.Connection.BeginTransaction())
                    {
                        try
                        {

                            foreach (sp_question_Result AD in AnswerDetail)
                            {
                                T_Inventory_Answer answer = new T_Inventory_Answer();
                                answer.Event_ID = ID;
                                answer.Inventory_Question_ID = AD.Inventory_Question_ID;
                                answer.Answer = AD.Answer;
                                answer.Status = true;
                                answer.Created_By = cwid;
                                answer.Created_Date = DateTime.Now;
                                db.T_Inventory_Answer.AddObject(answer);
                                db.SaveChanges();
                            }
                            trans.Commit();
                            db.Connection.Close();

                            return true;
                        }
                        catch(Exception ex)
                        {
                            trans.Rollback();
                            db.Connection.Close();
                            AuthorizedClient Obj = new AuthorizedClient();
                            Obj.SaveErrorLog("Save Answer Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                            return false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                AuthorizedClient Obj = new AuthorizedClient();
                Obj.SaveErrorLog("Save Answer Event", ex.Message, HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name, cwid);
                return false;
            }
        }

        public static List<sp_GetAnswer_Result> GetAnswer(int Event_ID)
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    return db.sp_GetAnswer().Where(a => a.Event_ID == Event_ID).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<sp_GetAnswer_Result> GetAnswer()
        {
            try
            {
                using (BayerDataPrivacyEntities db = new BayerDataPrivacyEntities())
                {
                    return db.sp_GetAnswer().ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
/**********************************************************
 * Demo for Standalone SDK.Created by Darcy on Oct.15 2009*
***********************************************************/
using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Data.Entity;
using MongoDB.Driver.Builders;
using System.Windows.Forms;

namespace OnlineEnroll
{
    public partial class rootsAuthapp
    {
        static TcpClient client;
        List<string> RTA = new List<string>();
        public rootsAuthapp()
        {
            OpenConnection();
          
           
           // while (true) ;
        
           
           
        }
        #region ack
        //void check_ack(string ip,int port)
        //{
        //    TcpClient tcpclntt = new TcpClient();
        //    tcpclntt.Connect(ip, port);
        //    string ack = "\0";
        //    Stream stmm = tcpclntt.GetStream();
        //    ASCIIEncoding asenn = new ASCIIEncoding();
        //    byte[] baa = asenn.GetBytes(ack);
        //    stmm.Write(baa, 0, baa.Length);

        //    byte[] bbb = new byte[100];
        //    int kk = stmm.Read(bbb, 0, 100);
        //}
        #endregion
        #region define_constants
        private static int machine_no = 1;

        #endregion
        #region defining_SDK
  
        public zkemkeeper.CZKEMClass machine1 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine2 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine3 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine4 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine5 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine6 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine7 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine8 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine9 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine10 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine11 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine12 = new zkemkeeper.CZKEMClass();
        public zkemkeeper.CZKEMClass machine13 = new zkemkeeper.CZKEMClass();

        #endregion
        #region Conncection
        bool bIsConnected = false;//the boolean value identifies whether the device is connected
        //If your device supports the TCP/IP communications, you can refer to this.
        //when you are using the tcp/ip communication,you can distinguish different devices by their IP address.
        void connectNow(string ip, int port, zkemkeeper.CZKEMClass machine)
        {
            bIsConnected = machine.Connect_Net(ip, port);
            btnConnect2_Click(machine);

        }
   
        void disconnectNow()
        {
             machine1.Disconnect();
        }
        #endregion
        #region OnlineEnroll
        private void create_new_employee_finger(string sName, string sPassword, int user_ID, int finger_index, int flag, int iPrivilege, bool bEnabled, int group_number)
        {

            int iUserID = Convert.ToInt32(user_ID);
            string sUserID = user_ID.ToString();
            machine1.CancelOperation();
            machine1.SSR_DelUserTmp(machine_no, sUserID, finger_index);//If the specified index of user's templates has existed ,delete it first.(SSR_DelUserTmp is also available sometimes)
            machine1.StartEnrollEx(sUserID, finger_index, flag);
            machine1.SSR_SetUserInfo(machine_no, user_ID.ToString(), sName, sPassword, iPrivilege, bEnabled);
            machine1.StartIdentify();
            assignEmpToGroup(group_number, user_ID);
            int iTmpLength = 0;
            byte[] byTmpData = new byte[2000];//modify by darcy on Dec.9 2009
            machine1.EnableDevice(machine_no, false);
            machine1.ReadAllTemplate(machine_no);
            while (machine1.SSR_GetUserInfo(machine_no, sUserID, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                if (machine1.GetUserTmpEx(machine_no, sUserID, finger_index, out flag, out byTmpData[0], out iTmpLength))
                {
                    
                    break;
                }
                
            }


         
        }
        private void add_emp_manually(int sdwEnrollNumber, string sName, string sPassword, int iPrivilege
                                      , bool bEnabled, int idwFingerIndex, int iFlag, int group_number, string HashTemp,[Optional] string faceHash,int requestID,string department)
        {
            int iFaceIndex = 50;//the only possible parameter value
            int iLength = 0;
             faceHash = "";
            machine1.EnableDevice(machine_no, false);
            machine1.SSR_SetUserInfo(machine_no, sdwEnrollNumber.ToString(), sName, sPassword, iPrivilege, bEnabled);//upload user information to the device
            machine1.SetUserTmpExStr(machine_no, sdwEnrollNumber.ToString(), idwFingerIndex, iFlag,HashTemp);//upload templates information to the device
            machine1.SetUserFaceStr(machine_no, sdwEnrollNumber.ToString(), iFaceIndex,faceHash, iLength);
            assignEmpToGroup(group_number, sdwEnrollNumber);
            machine1.EnableDevice(machine_no, true);
            machine1.RefreshData(machine_no);
        }




        public int sta_SetAllUserFPInfo(string sName, string sPassword, int iPrivilege, string sEnrollNumber, bool bEnabled, string sFPTmpData, string sCardnumber, int idwFingerIndex, int iFlag, string sFlag, string sdwFingerIndex, string sEnabled)
        {
            sCardnumber = "";   
            int num = 0;

            
            machine1.EnableDevice(machine_no, false);
            
                if (sEnabled == "true")
                {
                    bEnabled = true;
                }
                else
                {
                    bEnabled = false;
                }

                if (sCardnumber != "" && sCardnumber != "0")
                {
                    machine1.SetStrCardNumber(sCardnumber);
                }
                if (machine1.SSR_SetUserInfo(machine_no, sEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload user information to the device
                {
                    if (sdwFingerIndex != "" && sFlag != "" && sFPTmpData != "")
                    {
                        idwFingerIndex = Convert.ToInt32(sdwFingerIndex);
                        iFlag = Convert.ToInt32(sFlag);
                        machine1.SetUserTmpExStr(machine_no, sEnrollNumber, idwFingerIndex, iFlag, sFPTmpData);//upload templates information to the device
                    }
                    num++;
                  
                }
            

            machine1.RefreshData(machine_no);//the data in the device should be refreshed
            machine1.EnableDevice(machine_no, true);
            return 1;
        }

        #endregion
        #region Emp_info
        private void showAllEmployeeInfo(int requestID)
        {
            string sdwEnrollNumber = "";
            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            bool bEnabled = false;
            string ipvl;
            int idwFingerIndex;
            
            string FTmpData = "";
            int iTmpLength = 0;
            int iFlag = 0;
            int iFaceIndex = 50;//the only possible parameter value
            string sTmpData = "";
            string iFaceTemp="";
            
            int iLength = 0;

            machine1.EnableDevice(machine_no, false);
            machine1.ReadAllUserID(machine_no);//read all the user information to the memory
            machine1.ReadAllTemplate(machine_no);//read all the users' fingerprint templates to the memory
           
            List<string> empDataList = new List<string>();
            while (machine1.SSR_GetAllUserInfo(machine_no, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
            {
                
                ipvl = iPrivilege == 3 ? "Super Admin" : "Normal User";
                for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                {
                    
                    if (machine1.GetUserTmpExStr(machine_no, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))//get the corresponding templates string and length from the memory
                    {
                        
                        if (machine1.GetUserFaceStr(machine_no, sdwEnrollNumber, iFaceIndex, ref FTmpData, ref iLength))//get the face templates from the memory    
                        {
                         
                            iFaceTemp =FTmpData;
                            
                        }
                        else
                        {
                            
                            iFaceTemp = "Not Exist";
                           
                        }

                        //   empDataList.Add("{\n" + "\n \"UserID\" : " + $"\"{sdwEnrollNumber}\"" + ",\n \"Name\" : " + "\"" + sName + "\"" + ",\n\"finger Index\": " + $"\"{idwFingerIndex}\""
                        //                 + ",\n\"Authority\" : " + "\"" + ipvl + "\"" + ",\n\"Group\" : " + $"\"{getEmpGroup(int.Parse(sdwEnrollNumber))}\"" + ",\n\"Password\" : \n" + $"\"{sPassword}\"" + ",\n\"HashTemp\" : \n" + "" + $"\"{sTmpData}\"" +  "},");

                        empDataList.Add("{\n"+$"\"UserID\" : \"{sdwEnrollNumber}\" \n,\"Name\" : \"{sName}\",\n \"finger Index\" : \"{idwFingerIndex}\",\n\"Authority\": \"{ipvl}\",\n \"Group\" : \"{getEmpGroup(int.Parse(sdwEnrollNumber))}\",\n \"password\" : \"{sPassword}\",\n \"HashTemp\" : \"{sTmpData}\",\n \"FaceHash\" : \"{iFaceTemp}\" " +"}");


                    }

                }

            }
           
            string strEmpData = "["+string.Join(",", empDataList);
            
            SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + $",\n\"output\":{strEmpData}" + "]}}");
            machine1.EnableDevice(machine_no, true);
            
        }
        private void deleteEmp(int userID, int iBackupNumber,int requestID)
        {
            int finger_index = 0;
            for (finger_index = 0; finger_index < 10; finger_index++)
            {
                machine1.SSR_DelUserTmpExt(machine_no, userID.ToString(), finger_index);
                machine1.SSR_DeleteEnrollData(machine_no, userID.ToString(), iBackupNumber);
                
            }
            SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + $",\n\"output\":\"The Emp {userID} has been deleted successfully\"" + "}\n}");
        }
        private void deleteEmpSpecificFinger(int userID, int specific_finger)
        {
            machine1.SSR_DelUserTmpExt(machine_no, userID.ToString(), specific_finger);
            machine1.RefreshData(machine_no);//the data in the device should be refreshed          

          
        }
        private void setOrRemoveAdmin(int userID, bool admin,int requestID)
        {
            if (admin == true)
                SendData("{"+$"\"requestNumber\":\"{requestID}\",\n" +$"\"respond\":"+"{\n"+ $"\"status\":{true}" + $"\n\"Data\":someone set{userID} as admin ");
            else
                SendData("{" + $"\"requestNumber\":\"{requestID}\",\n" + $"\"respond\":" + "{\n" + $"\"status\":{true}" + $"\n\"Data\":someone set{userID} as Normal User ");
            machine1.SSR_EnableUser(machine_no, userID.ToString(), admin);
            machine1.RefreshData(machine_no);//the data in the device should be refreshed   
            

        }
        public void uploadOneUserPhoto( string fullName, int requestID)
        {
            machine1.EnableDevice(machine_no, false);
            machine1.UploadUserPhoto(machine_no, fullName);
            machine1.RefreshData(machine_no);//the data in the device should be refreshed           
            machine1.SendFile(machine_no, fullName);
                machine1.RefreshData(machine_no);
            machine1.EnableDevice(machine_no, true);//enable the device
        
            SendData("{\nrequestID:" + requestID + ",\nrespond{\n operation:\"the process has successfully finished\"" + "}\n}");


        }

        #endregion
        #region Access control
        public int assignEmpToSpecialTimeZone(int UserID, int TZtype, [Optional] string txtUTZIndex1, [Optional] string txtUTZIndex2, [Optional] string txtUTZIndex3,int requestID)
        {
            int iTZ1 = 0;
            int iTZ2 = 0;
            int iTZ3 = 0;

            if (TZtype == 0)
            {
                txtUTZIndex1 = "";
                txtUTZIndex2 = "";
                txtUTZIndex3 = "";
            }
            else
            {

                iTZ1 = Convert.ToInt32(txtUTZIndex1);
                iTZ2 = Convert.ToInt32(txtUTZIndex2);
                iTZ3 = Convert.ToInt32(txtUTZIndex3);

                if (iTZ1 < 0 || iTZ1 > 50 || iTZ2 < 0 || iTZ2 > 50 || iTZ3 < 0 || iTZ3 > 50)
                {

                    SendData("{\nrequestID:" + requestID + ",\nrespond{\n operation:\"Timezone index error!\"" + "}\n}");
                    

                    return -1022;
                }
            }

            int idwErrorCode = 0;

            string sTZs = iTZ1.ToString() + ":" + iTZ2.ToString() + ":" + iTZ3.ToString() + ":" + TZtype.ToString();

            if (machine1.SetUserTZStr(machine_no, UserID, sTZs))//TZs is in strings.
            {
                machine1.RefreshData(machine_no);//the data in the device should be refreshed
                SendData("{\nrequestID:" + requestID + ",\nrespond{\n operation:\"Set user TZ successfully\"" + "}\n}");
                
            }
            else
            {
                machine1.GetLastError(ref idwErrorCode);
                SendData("{\nrequestID:" + requestID + ",\nrespond{\n operation:\"" + "Operation failed,ErrorCode=" + idwErrorCode.ToString() + "\"" + "}\n}");
              

                return idwErrorCode;
            }
            return 1;
        }

        public int setTimeZone(int TZIndex, string dtSUNs, string dtMONs, string dtTUEs, string dtWENs, string dtTHUs, string dtFRIs, string dtSATs, string dtSUNe, string dtMONe, string dtTUEe, string dtWENe, string dtTHUe, string dtFRIe, string dtSATe,int requestID)
        {

            int idwErrorCode = 0;
            int iTimeZoneID = Convert.ToInt32(TZIndex);


            DateTime dSUNs = DateTime.Parse(dtSUNs);
            DateTime dMONs = DateTime.Parse(dtMONs);
            DateTime dTUEs = DateTime.Parse(dtTUEs);
            DateTime dWENs = DateTime.Parse(dtWENs);
            DateTime dTHUs = DateTime.Parse(dtTHUs);
            DateTime dFRIs = DateTime.Parse(dtFRIs);
            DateTime dSATs = DateTime.Parse(dtSATs);

            DateTime dSUNe = DateTime.Parse(dtSUNe);
            DateTime dMONe = DateTime.Parse(dtMONe);
            DateTime dTUEe = DateTime.Parse(dtTUEe);
            DateTime dWENe = DateTime.Parse(dtWENe);
            DateTime dTHUe = DateTime.Parse(dtTHUe);
            DateTime dFRIe = DateTime.Parse(dtFRIe);
            DateTime dSATe = DateTime.Parse(dtSATe);

            string sSunTime = dSUNs.Hour.ToString("00") + dSUNs.Minute.ToString("00") + dSUNe.Hour.ToString("00") + dSUNe.Minute.ToString("00");
            string sMonTime = dMONs.Hour.ToString("00") + dMONs.Minute.ToString("00") + dMONe.Hour.ToString("00") + dMONe.Minute.ToString("00");
            string sTueTime = dTUEs.Hour.ToString("00") + dTUEs.Minute.ToString("00") + dTUEe.Hour.ToString("00") + dTUEe.Minute.ToString("00");
            string sWenTime = dWENs.Hour.ToString("00") + dWENs.Minute.ToString("00") + dWENe.Hour.ToString("00") + dWENe.Minute.ToString("00");
            string sThuTime = dTHUs.Hour.ToString("00") + dTHUs.Minute.ToString("00") + dTHUe.Hour.ToString("00") + dTHUe.Minute.ToString("00");
            string sFriTime = dFRIs.Hour.ToString("00") + dFRIs.Minute.ToString("00") + dFRIe.Hour.ToString("00") + dFRIe.Minute.ToString("00");
            string sSatTime = dSATs.Hour.ToString("00") + dSATs.Minute.ToString("00") + dSATe.Hour.ToString("00") + dSATe.Minute.ToString("00");

            string sTimeZone = sSunTime + sMonTime + sTueTime + sWenTime + sThuTime + sFriTime + sSatTime;

            if (machine1.SetTZInfo(machine_no, iTimeZoneID, sTimeZone))
            {
                //the data in the device should be refreshed
                machine1.RefreshData(machine_no);
                SendData("{\nrequestID:" + requestID + ",\nrespond:{" + "\"Timezone has been set successfully !\"" + "}}");
                

            }
            else
            {
                machine1.GetLastError(ref idwErrorCode);
                if (idwErrorCode == -12008)
                {
                 
                    SendData("{\nrequestID:" + requestID + ",\nrespond:{" + "\"Over TimeZoneIndex limits!\"" + "}}");
                    
                }
                else if (idwErrorCode == 4)
                {

                    SendData("{\nrequestID:" + requestID + ",\nrespond:{" + "\"TimeZone Format Error!\"" + "}}");
                    
                }
                else
                {

                    SendData("{\nrequestID:" + requestID + ",\nrespond:{" + "\""+ "Operation failed, ErrorCode = " + idwErrorCode.ToString()+"\"" + "}}");
                    
                }
            }
            return 1;
        }

        public void getTimeZone(int txtTZIndex,int requestID)
        {
            string error = "";
            string dtSUNs;
            string dtMONs;
            string dtTUEs;
            string dtWENs;
            string dtTHUs;
            string dtFRIs;
            string dtSATs;
            string dtSUNe;
            string dtMONe;
            string dtTUEe;
            string dtWENe;
            string dtTHUe;
            string dtFRIe;
            string dtSATe;

            int idwErrorCode = 0;

            if (txtTZIndex <= 0 || txtTZIndex > 50)
            {
                SendData("{\nrequestID:" + requestID + ",\nrespond:{" + "\"Timezone index error!\"" + "}}");

            }

            string sTimeZone = "";

            if (machine1.GetTZInfo(machine_no, txtTZIndex, ref sTimeZone))
            {
                
                string[] array = new string[sTimeZone.Length / 2];
                int i, j = 0;
                for (i = 0; (i + 2) <= sTimeZone.Length && sTimeZone.Length >= i;)
                {
                    array[j] = sTimeZone.Substring(i, 2);
                    j++;
                    i = i + 2;
                }

                dtSUNs = array[0] + ":" + array[1];
                dtSUNe = array[2] + ":" + array[3];

                dtMONs = array[4] + ":" + array[5];
                dtMONe = array[6] + ":" + array[7];

                dtTUEs = array[8] + ":" + array[9];
                dtTUEe = array[10] + ":" + array[11];

                dtWENs = array[12] + ":" + array[13];
                dtWENe = array[14] + ":" + array[15];

                dtTHUs = array[16] + ":" + array[17];
                dtTHUe = array[18] + ":" + array[19];

                dtFRIs = array[20] + ":" + array[21];
                dtFRIe = array[22] + ":" + array[23];

                dtSATs = array[24] + ":" + array[25];
                dtSATe = array[26] + ":" + array[27];
                string res = "{" + "\nSaturday:" + "\"" + dtSATs + "-" + dtSATe + "\","
                    + "\nSunday:" + "\"" + dtSUNs + "-" + dtSUNe + "\","
                    + "\nMonday:" + "\"" + dtMONs + "-" + dtMONe + "\","
                    + "\nTuesday:" + "\"" + dtTUEs + "-" + dtTUEe + "\","
                    + "\nWednsday:" + "\"" + dtWENs + "-" + dtWENe + "\","
                    + "\nThursday:" + "\"" + dtTHUs + "-" + dtTHUe + "\","
                    + "\nFriday:" + "\"" + dtFRIs + "-" + dtFRIe + "\"," + "\n}";
                SendData("{\nrequestID:" + requestID + ",\nrespond:{" + "\"" + res + "\"" + "}}");

            }
            else
            {
                machine1.GetLastError(ref idwErrorCode);
                if (idwErrorCode == -2 || idwErrorCode == -12008)
                {
                    error = "Operation failed,ErrorCode=" + idwErrorCode.ToString() + ",Over TimeZoneIndex limits!";
                    
                }
                else
                {
                    error = "*Operation failed,ErrorCode=" + idwErrorCode.ToString();
                    
                }
                SendData("{\nrequestID:" + requestID + ",\nrespond:{" + "\"" + error + "\"" + "}}");

            }


        }
        private void createAccessableGroup(int grp,bool holiday,int timeZone1, int timeZone2, int timeZone3,int VS,int requestID)
        {
            machine1.SSR_SetUnLockGroup(machine_no, 1, grp, 0, 0, 0, 0);
            machine1.SSR_SetGroupTZ(machine_no, grp, timeZone1, timeZone2,timeZone3,holiday==true?1:0,VS);  
            machine1.RefreshData(machine_no);//the data in the device should be refreshed
            SendData("{\nrequestID:" + requestID + ",\nrespond{\n operation:\"the process has successfully finished\"" + "}\n}");

        }
        private void createNoneAccessableGroup(int grp, bool holiday, int timeZone1, int timeZone2, int timeZone3, int VS,int requestID)
        {
            machine1.SSR_SetGroupTZ(machine_no, grp, timeZone1, timeZone2, timeZone3, holiday == true ? 1 : 0, VS);
            machine1.RefreshData(machine_no);//the data in the device should be refreshed
            SendData("{\nrequestID:" + requestID + ",\nrespond{\n operation:\"the process has successfully finished\"" + "}\n}");
        }
        void assignEmpToGroup(int gp, int ID) {
            machine1.SetUserGroup(machine_no, ID, gp);
            machine1.RefreshData(machine_no);//the data in the device should be refreshed
      


        }
        private int getEmpGroup(int iUserID)
        {

            int iUserGrp = 0;
            machine1.GetUserGroup(machine_no, iUserID, ref iUserGrp);
            return iUserGrp;


        }
 
        private void DenyEmp(int iD,int requestID) 
                                      //group two mostly recommended
        {
            assignEmpToGroup(2, iD);
            machine1.RefreshData(machine_no);//the data in the device should be refreshed
            SendData("{\nrequestID:" + requestID + ",\nrespond{\n operation:\"the process has successfully finished\"" + "}\n}");

        }
        private void ApproveEmp(int iD) //group one is premitted by defualt
        {
            assignEmpToGroup(1, iD);
            machine1.RefreshData(machine_no);//the data in the device should be refreshed
        }
        void OpenCloseDoor(int sec,int requestID)
        {

            machine1.ACUnlock(machine_no, sec);
            SendData("{\n\"requestID\": " + requestID+ ",\n\"respond\" :{\n\"status\":"+ "1" + ",\n\"output\":\"The door opened successfully\"" + "}\n}");
           


        }

        private void Create_Holiday(int iHolidayID, int sday, int smonth, int Eday, int Emonth,int requestID)
        {
            machine1.SSR_SetHoliday(machine_no, iHolidayID, smonth, sday, Emonth, Eday, 1);
            machine1.RefreshData(machine_no);//the data in the device should be refreshed
            SendData("{\nrequestID:" + requestID + ",\nrespond{\n operation:\"the holiday has successfully created\"" + "}\n}");
        }
        private void Get_Holiday(int ID,int requestID)
        {
            int iBM = 0;//begining month
            int iBD = 0;//begining day
            int iEM = 0;//ending month
            int iED = 0;//ending day
            machine1.SSR_GetHoliday(machine_no, ID, ref iBM, ref iBD, ref iEM, ref iED, 1);
            string res = "{" + "Date : \"" + iBD + "/" + iBM + " - " + iED + "/" + iEM + "\"" + "}";
            SendData("{\nrequestID:" + requestID + ",\nrespond:{" + "\""+res+"\"" + "}}");

        }
        public void GetGroupTZ(int GroupNo, int requestID)
        {
            string TZIndex1;
            string TZIndex2;
            string TZIndex3;
            string cboACValidHoliday;
            string cbVerifyStyle;

            int iValidHoliday = 0;
            int iVerifyStyle = 0;
            int iTZ1 = 0;
            int iTZ2 = 0;
            int iTZ3 = 0;

            if (machine1.SSR_GetGroupTZ(machine_no, GroupNo, ref iTZ1, ref iTZ2, ref iTZ3, ref iValidHoliday, ref iVerifyStyle))
            {
                TZIndex1 = iTZ1.ToString();
                TZIndex2 = iTZ2.ToString();
                TZIndex3 = iTZ3.ToString();
                cboACValidHoliday = iValidHoliday.ToString();
                cbVerifyStyle = iVerifyStyle.ToString();
                SendData("{/n requestID:" + requestID + ",\nrespond:\n{\n timeZone1:" + TZIndex1 + ",\n timeZone2:" + TZIndex2 + ",\n timeZone3:" + TZIndex3 + ",\n OnOffHolidays:" + cboACValidHoliday + ",\n verifyStyle:" + cbVerifyStyle + "\n}\n}");
                SendData(TZIndex1 + TZIndex2 + TZIndex3 + cboACValidHoliday + cbVerifyStyle);
            }


        }

        #endregion
        #region log_list
        private void Get_Log_Data(int requestID)
        {
            string sdwEnrollNumber = "";
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwWorkcode = 0;
            int iGLCount = 0;
            //int iIndex = 0;
            string time;
            machine1.EnableDevice(machine_no, false);//disable the device
            #region Variables
            int idwSecond;
            #endregion
            List<string> EmpLogData = new List<string>();
            
            while (machine1.SSR_GetGeneralLogData(machine_no, out sdwEnrollNumber, out idwVerifyMode,
                           out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
            { 
                    
                    iGLCount++;
                time = idwDay + "-" + idwMonth + "-" + idwYear + " " + idwHour + ":" + idwMinute;
                if (idwInOutMode == 0)
                {
                    EmpLogData.Add("\n{" + "\n\"seq\" : " + $"\"{iGLCount}\"" + ",\n \"ID\" :" + $"\"{sdwEnrollNumber}\"" + $",\n \" Time of Check in \" : \" {time} \"" + "\n}");
                }
                else {

                    EmpLogData.Add("\n{" + "\n\"seq\" : " + $"\"{iGLCount}\"" + ",\n \"ID\" :" + $"\"{sdwEnrollNumber}\"" + $",\n \" Time of Check out \" : \" {time} \"" + "\n}");
                }
                    
                

            }
            
            string strEmpLogData = "[" + string.Join(",", EmpLogData);
           
            SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + $",\n\"output\":{strEmpLogData}" + "]}}");
            machine1.EnableDevice(machine_no, true);//enable the device

        }
        private void Get_Log_Data_specific_date(int day, int month, int year,int requestID)
        {
            string sdwEnrollNumber = "";
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            int idwWorkcode = 0;
            int iGLCount = 0;
            int iIndex = 0;
            string time;
            machine1.EnableDevice(machine_no, false);//disable the device
            List<string> EmpLogSpecficDate = new List<string>();
           
            //tracing all logs 
            while (machine1.SSR_GetGeneralLogData(machine_no, out sdwEnrollNumber, out idwVerifyMode,
                           out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
            {
                time = idwDay + "-" + idwMonth + "-" + idwYear + " " + idwHour + ":" + idwMinute;
               
                iGLCount++;
                if (idwDay == day && idwMonth == month && idwYear == year&&idwInOutMode==0) {
                    EmpLogSpecficDate.Add("\n{" + "\n\"seq\" : " + $"\"{iGLCount}\"" + ",\n \"nUserID\" :" + $"\"{sdwEnrollNumber}\"" + $",\n \" Time of Check in \" : \" {time} \"" + "\n}");
                }
                else if (idwDay == day && idwMonth == month && idwYear == year && idwInOutMode == 1)
                {
                    EmpLogSpecficDate.Add("\n{" + "\n\"seq\" : " + $"\"{iGLCount}\"" + ",\n \"nUserID\" :" + $"\"{sdwEnrollNumber}\"" + $",\n \" Time of Check out \" : \" {time} \"" + "\n}");
                }

                iIndex++;
            }
          
            string strEmpLogspecificDate = "["+string.Join(",", EmpLogSpecficDate);
            SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + $",\n\"output\":{strEmpLogspecificDate}" + "]}}");
            machine1.EnableDevice(machine_no, true);//enable the device

        }
        private void GetLogDataspecificID(int UserID,int requestID)
        {
            string sdwEnrollNumber = "";
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            int idwWorkcode = 0;
            int iGLCount = 0;
            int iIndex = 0;
            string time;
            machine1.EnableDevice(machine_no, false);//disable the device
           
            List<string> EmpLogDataSpecificID = new List<string>();

            //tracing all logs 
            while (machine1.SSR_GetGeneralLogData(machine_no, out sdwEnrollNumber, out idwVerifyMode,
                           out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
            {
                iGLCount++;
                time = idwDay + "-" + idwMonth + "-" + idwYear + " " + idwHour + ":" + idwMinute;
                if (UserID.ToString() == sdwEnrollNumber && idwInOutMode == 0)
                {
                    EmpLogDataSpecificID.Add("\n{" + "\n\"seq\" : " + $"\"{iGLCount}\"" + ",\n \"nUserID\" :" + $"\"{sdwEnrollNumber}\"" + $",\n \" Time of Check in \" : \" {time} \"" + "\n}");
                }
                else if (UserID.ToString() == sdwEnrollNumber && idwInOutMode == 1)
                {
                    EmpLogDataSpecificID.Add("\n{" + "\n\"seq\" : " + $"\"{iGLCount}\"" + ",\n \"nUserID\" :" + $"\"{sdwEnrollNumber}\"" + $",\n \" Time of Check out \" : \" {time} \"" + "\n}");
                }
                iIndex++;
            }
           
            string strEmpLogDataSpecificID = "["+string.Join(",", EmpLogDataSpecificID);
            SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + $",\n\"output\":{strEmpLogDataSpecificID}" + "]}}");
            machine1.EnableDevice(machine_no, true);//enable the device

        }
        #endregion
        #region Device_options
        void reset_fact(int requestID) {

            machine1.ClearData(machine_no, 5);
            machine1.ClearData(machine_no, 2);
            machine1.ClearAdministrators(machine_no);
            //machine1.ClearSLog(machine_no);
            //machine1.ClearKeeperData(machine_no);
            machine1.RefreshData(machine_no);
            SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + ",\n\"output\":\"someone has resetted machine number"+$"{machine_no}\"" + "}\n}");
        }
        private void DeviceStrInfo(int requestID)
        {
            string sValue = "";
            machine1.GetDeviceStrInfo(machine_no, 1, out sValue);
            SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + ",\n\"output\":\"Device information has checked machine number" + $"{machine_no}\"" + "}\n}");
        }
        private void GetFirmwareVersion(int requestID)
        {
            string sVersion = "";
            machine1.GetFirmwareVersion(machine_no, ref sVersion);
            SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + ",\n\"output\":\"Firmware version has checked for machine number" + $"{machine_no}\"" + "}\n}");
        }
        private void GetSerialNumber(int requestID)
        {
            string sdwSerialNumber = "";
            machine1.GetSerialNumber(machine_no, out sdwSerialNumber);
            SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + ",\n\"output\":\"Serial number has been checked for machine number" + $"{machine_no}\"" + "}\n}");

        }
        private void btnSetCommPassword_Click(int iCommKey)
        {
            machine1.SetCommPassword(iCommKey);
            machine1.RefreshData(machine_no);

        }
        private string GetDeviceMAC()
        {

            string sMAC = "";
            machine1.GetDeviceMAC(machine_no, ref sMAC);
            return sMAC;
        }
        private void DisableDeviceWithTimeOut(int timeInSecond,int requestID)
        {
            machine1.DisableDeviceWithTimeOut(machine_no, timeInSecond);
            machine1.RefreshData(machine_no);//the data in the device should be refreshed
            SendData("{\n\"requestID\" :" + "\"" + requestID + "\"" + ",\n\"respond\":{\n" + "\"Data\" :" + "\"" + "No Data" + "\"\n" + ",\"operation\": " + $"\"someone disabled the machine number {machine_no} for {timeInSecond} seconds \"" + "}}");
            
        }
        private void RestartDevice(int requestID)
        {
            machine1.RestartDevice(machine_no);
            
            SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + ",\n\"output\":\"someone restarted the machine number" + $"{machine_no}\"" + "}\n}");


        }
        private void PowerOffDevice(int requestID)
        {    
            machine1.PowerOffDevice(machine_no);
            machine1.Disconnect();
            SendData("{\n\"requestID\" :" + "\"" + requestID + "\"" + ",\n\"respond\":{\n" + "\"Data\" :" + "\"" + "No Data" + "\"\n" + ",\"operation\": " + $"\"someone powered the machine {machine_no} off \"" + "}}");
        }
        void illegalBeep(int beepsound, int requestID) {
            /*
             * 0:Thank you
             * 1:incorrect password
             * 2:Access denied
             * 3:invalid ID
             * 
            */
            machine1.PlayVoiceByIndex(beepsound);
            SendData("{\nrequestID:" + requestID + ",\nrespond:{" + "\"The request has successfully done\"" + "}}");
        }
 
        private void soundTheRing(int sec)
        {
            machine1.Beep(sec * 1000);
        }

        #endregion
        #region setup wallpaper&advertisments

        void upload_and_set_wallpaper(string path, int requestID) {
            machine1.EnableDevice(machine_no, false);//disable the device
            machine1.UploadTheme(machine_no, path, "wallpaper1.png");
            machine1.EnableDevice(machine_no, true);//disable the device
            SendData("{\nrequestID:" + requestID + ",\nrespond{\n operation:\"the process has successfully finished\"" + "}\n}");
        }
        void upload_and_set_advertise(string path, string adv, int requestID) {
            machine1.EnableDevice(machine_no, false);//disable the device
            machine1.UploadPicture(machine_no, path, adv);
            machine1.EnableDevice(machine_no, true);
            SendData("{\nrequestID:" + requestID + ",\nrespond{\n operation:\"the process has successfully finished\"" + "}\n}");
        }
        #endregion
        #region actionsAndErroLogs
        public int sta_GetOplog(int requestID)
        {
            int ret = 0;
            int iSuperLogCount = 0;

            machine1.EnableDevice(machine_no, false);
            List<string> OpList = new List<string>();
            if (machine1.ReadAllSLogData(machine_no))
            {
                int idwTMachineNumber = 0;
                int iParams1 = 0;
                int iParams2 = 0;
                int idwManipulation = 0;
                int iParams3 = 0;

                int iParams4 = 0;
                int iYear = 0;
                int iMonth = 0;
                int iDay = 0;
                int iHour = 0;
                int iMin = 0;
                int iSencond = 0;
                int iAdmin = 0;
                string sTime = null;
                String count, admin, operation, ID, opResult, Log_str;
                
                while (machine1.GetSuperLogData2(machine_no, ref idwTMachineNumber, ref iAdmin, ref iParams4, ref iParams1, ref iParams2, ref idwManipulation, ref iParams3, ref iYear, ref iMonth, ref iDay, ref iHour, ref iMin, ref iSencond))
                {
                    iSuperLogCount++;
                    count = iSuperLogCount.ToString();
                    admin = iAdmin.ToString();
                    switch (idwManipulation)
                    {
                        case 34:
                            operation = "Illegal Access";
                            break;
                        case 4:
                            operation = "Menu Accessed";
                            break;
                        case 0:
                            operation = "Machine restarted";
                            break;
                        case 1:
                            operation = "Machine shutted down";
                            break;
                        case 71:
                            operation = "change admin -> user ";
                            break;
                        case 36:
                            operation = "change user -> admin ";
                            break;
                        case 37:
                            operation = "Trying to access outside the authorized timezone";
                            break;
                        case 6:
                            operation = "New Fingerprint record added ";
                            break;
                        case 9: 
                            operation = "A record has been deleted ";
                            break;
                        default:
                            operation = idwManipulation.ToString();
                            break;
                    }
                    sTime = iYear + "-" + iMonth + "-" + iDay + " " + iHour + ":" + iMin + ":" + iSencond;

                    ID = iParams1.ToString();
                    switch (iParams2)
                    {
                        case 0:
                            opResult = "Success";
                            break;
                        default:
                            opResult = "failed";
                            break;
                    }

                    Log_str = "\n{\n" + $"\"Count\" :\"{count}\",\n\"MachineNo\" : \"{machine_no}\" ,\n\"Admin\" :  \"{ admin}\",\n\"Operaion\" : \"{operation}\" ,\n\"Time\" :   \"{sTime}\",\n\"ID\" : \"{ID}\" ,\n\"OperationResult\" :   \"{opResult}\""+"}";
                    OpList.Add(Log_str);


                }

            }

            string strOpList = "["+string.Join(",", OpList);
            
            SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + $",\n\"output\":{strOpList}" + "]}}");
            machine1.EnableDevice(machine_no, true);
            return ret;
        }


        #endregion
        #region communication_with_node
        void ExcFunction(string fullStr)
        {
            string code = fullStr.Substring(0, fullStr.IndexOf("{")-1);
            string jsonString = fullStr.Substring(fullStr.IndexOf("{"));
            JObject json = JObject.Parse(jsonString);
            var pram = json["prm"];
            var ipsec = json["Device"];

            connectNow(ipsec["ip"].ToString(), Convert.ToInt32(ipsec["port"]),machine1);
            connectNow(ipsec["ip2"].ToString(), Convert.ToInt32(ipsec["port2"]), machine2);

            switch (code)
            {
                case "0":
                    OpenCloseDoor(10, Convert.ToInt32(json["requestID"]));

                    break;
                case "1":    
                    showAllEmployeeInfo(Convert.ToInt32(json["requestID"]));
                    break;
                case "2":
                    Get_Log_Data(Convert.ToInt32(json["requestID"]));
                    break;
                case "3":
                    
                    Get_Log_Data_specific_date(Convert.ToInt32(pram["Day"]), Convert.ToInt32(pram["Month"]), Convert.ToInt32(pram["Year"]), Convert.ToInt32(json["requestID"]));    
                    break;
                case "4":
                    GetLogDataspecificID(Convert.ToInt32(pram["UserID"]), Convert.ToInt32(json["requestID"]));   
                    break;
                case "5":
                    reset_fact(Convert.ToInt32(json["requestID"]));
                    break;
                case "6":
                    RestartDevice(Convert.ToInt32(json["requestID"]));
                    break;
                case "7":
                 
                    sta_GetOplog(Convert.ToInt32(json["requestID"]));             
                    break;
                case "8":
                    create_new_employee_finger(pram["sName"].ToString(), pram["sPassword"].ToString(), Convert.ToInt32(pram["UserID"]), Convert.ToInt32(pram["FingerIndex"]), Convert.ToInt32(pram["Flag"]), Convert.ToInt32(pram["Priv"]), Convert.ToBoolean(pram["Enabled"]), Convert.ToInt32(pram["Group"]));
                    SendData("{\n\"requestID\": " + Convert.ToInt32(json["requestID"]) + ",\n\"respond\" :{\n\"status\":" + "1" + ",\n\"output\":\"An Employee has successfully created with ID"+$"{Convert.ToInt32(pram["UserID"])}\"" + "}\n}");
                   
                    break;
                case "9":        
                    add_emp_manually(Convert.ToInt32(pram["UserID"]), pram["sName"].ToString(), pram["sPassword"].ToString(), Convert.ToInt32(pram["Priv"].ToString()), Convert.ToBoolean(pram["Enabled"]), Convert.ToInt32(pram["FingerIndex"]), Convert.ToInt32(pram["Flag"]), Convert.ToInt32(pram["Group"]), pram["HashTemp"].ToString(), pram["FaceHash"].ToString(), Convert.ToInt32(json["requestID"]), pram["Department "].ToString());
                    SendData("{\nrequestID:" + Convert.ToInt32(json["requestID"]) + ",\nrespond{\n operation:\"The Employee data enrolled successfully\"" + "\n}\n}");
                    //addtoDB("Enrollment","respond","{\nrequestID:" + Convert.ToInt32(json["requestID"]) + ",\nrespond{\n operation:\"The Employee data enrolled successfully\"" + "\n}\n}");
                   
                    break;
                case "10":
                    GetGroupTZ(Convert.ToInt32(pram["Group"]), Convert.ToInt32(json["requestID"]));
                    break;
                case "11":
                    SendData("{\n\"requestID\" :" + Convert.ToInt32(json["requestID"]) + ",\n\"respond\":{\n" + "\"Data\" :" + "\"" + GetDeviceMAC() + "\"\n" + ",\"operation\" :" + " \"someone requested the Device Mac Adress\" " + "}}");
                  //  SendData("{\n\"requestID\": " + requestID + ",\n\"respond\" :{\n\"status\":" + "1" + ",\n\"output\":\"someone has resetted machine number" + $"{machine_no}\"" + "}\n}");

                    break;
                case "12":
                    //error here
                    PowerOffDevice(Convert.ToInt32(json["requestID"]));
                    break;
                case "13":
                    DeviceStrInfo(Convert.ToInt32(json["requestID"]));
                    break;
                case "14":
                    GetFirmwareVersion(Convert.ToInt32(json["requestID"]));
                    break;
                case "15":
                    DisableDeviceWithTimeOut(Convert.ToInt32(pram["Timeout"]), Convert.ToInt32(json["requestID"]));
                    break;
                case "16":
                    GetSerialNumber(Convert.ToInt32(json["requestID"]));
                    break;
                case "17":
                    setTimeZone(Convert.ToInt32(pram["TimeZone1"]), pram["dtSUNs"].ToString(), pram["dtMONs"].ToString(),
                       pram["dtTUEs"].ToString(), pram["dtWENs"].ToString(), pram["dtTHUs"].ToString(), pram["dtFRIs"].ToString(),
                        pram["dtSATs"].ToString(), pram["dtSUNe"].ToString(), pram["dtMONe"].ToString(), pram["dtTUEe"].ToString(),
                        pram["dtWENe"].ToString(), pram["dtTHUe"].ToString(), pram["dtFRIe"].ToString(), pram["dtSATe"].ToString(), Convert.ToInt32(json["requestID"]));
                    break;
                case "18":
                    getTimeZone(Convert.ToInt32(pram["TimeZone1"]), Convert.ToInt32(json["requestID"]));
                    break;
                case "19":
                    ApproveEmp(Convert.ToInt32(pram["UserID"]));
                    SendData("{\nrequestID:" + Convert.ToInt32(json["requestID"]) + ",\nrespond{\n operation:\"the process has successfully finished\"" + "}\n}");
                    break;
                case "20":
                    DenyEmp(Convert.ToInt32(pram["UserID"]), Convert.ToInt32(json["requestID"]));
                    break;
                case"21":
                    createAccessableGroup(Convert.ToInt32(pram["Group"]), Convert.ToBoolean(pram["HolidayOnOff"]), Convert.ToInt32(pram["TimeZone1"]), Convert.ToInt32(pram["TimeZone2"]), Convert.ToInt32(pram["TimeZone3"]), Convert.ToInt32(pram["VerifyStyle"]), Convert.ToInt32(json["requestID"]));
                        break;
                case "22":
                    createNoneAccessableGroup(Convert.ToInt32(pram["Group"]), Convert.ToBoolean(pram["HolidayOnOff"]), Convert.ToInt32(pram["TimeZone1"]), Convert.ToInt32(pram["TimeZone2"]), Convert.ToInt32(pram["TimeZone3"]), Convert.ToInt32(pram["VerifyStyle"]), Convert.ToInt32(json["requestID"]));
                    break;
                case "23":
                    assignEmpToGroup(Convert.ToInt32(pram["Group"]), Convert.ToInt32(pram["UserID"]));
                    Convert.ToInt32(json["requestID"]);
                    break;
                case "24":
                    //GetEmpGroup()
                    SendData("{\nrequestID:" + Convert.ToInt32(json["requestID"]) + ",\nrespond:{" + "\"" + getEmpGroup(Convert.ToInt32(pram["UserID"])) + "\"" + "}}");
                    break;
                case "25":
                    Create_Holiday(Convert.ToInt32(pram["HolidayId"]), Convert.ToInt32(pram["StartDayHoliDay"]), Convert.ToInt32(pram["StartMonthHoliday"]), Convert.ToInt32(pram["EndDayHoliDay"]), Convert.ToInt32(pram["EndMonthHoliday"]), Convert.ToInt32(json["requestID"]));
                    break;
                case "26":
                    Get_Holiday(Convert.ToInt32(pram["HolidayId"]), Convert.ToInt32(json["requestID"]));
                    break;
                     
                case "27":
                    //error here
                    deleteEmp(Convert.ToInt32(pram["UserID"]), Convert.ToInt32(pram["Backup"]), Convert.ToInt32(json["requestID"]));
                    break;
                case "28":
                    assignEmpToSpecialTimeZone(Convert.ToInt32(pram["UserID"]), Convert.ToInt32(pram["CustomizeEmpToTZ"]),pram["TimeZone1"].ToString(),pram["TimeZone2"].ToString(), pram["TimeZone3"].ToString(), Convert.ToInt32(json["requestID"]));
                    break;
                case "29":
                    illegalBeep(Convert.ToInt32(pram["BeepSoundindex"]), Convert.ToInt32(json["requestID"]));
                    break;
                case "30" :
                    GetLogDataspecificID(Convert.ToInt32(pram["UserID"]), Convert.ToInt32(json["requestID"]));
                    break;
                case "31":
                    uploadOneUserPhoto(pram["FilePath"].ToString(), Convert.ToInt32(json["requestID"]));
                    break;
                case "32":
                    upload_and_set_wallpaper(pram["FilePath"].ToString(), Convert.ToInt32(json["requestID"]));
                    break;
                case "33":
                    upload_and_set_advertise(pram["FilePath"].ToString(), pram["AdvName"].ToString(), Convert.ToInt32(json["requestID"]));
                    break;
                case "34":
                    break;
                case "35":
                   
                    break;
                default:
                    SendData("{\"Error:\":\"INVALID CHOICE\"}");
                    break;
            }
           // disconnectNow();
        }
        void OpenConnection()
        {
            client = new TcpClient();
            client.Connect(IPAddress.Loopback, 3000);
            NetworkStream clientStream = client.GetStream();
            System.Threading.Thread.Sleep(1000);//Sleep before we get the data for 1 second 
            byte[] inMessage;
            int bytesRead;
            string chr="";
            
            while (true)
            {
                if (clientStream.DataAvailable)
                {
                    
                    inMessage = new byte[4096];
                    bytesRead = clientStream.Read(inMessage, 0, 4096);
                    ASCIIEncoding encoder = new ASCIIEncoding();
                    chr = encoder.GetString(inMessage, 0, bytesRead);     
                    ExcFunction(chr);
                    client.Client.Disconnect(false);


                }


            }

            
              



        }

        #region Communication
        //the serial number of the device.After connecting the device ,this value will be changed.

        //If your device supports the TCP/IP communications, you can refer to this.
        //when you are using the tcp/ip communication,you can distinguish different devices by their IP address.
    
        private void btnConnect2_Click(zkemkeeper.CZKEMClass machine)
        {

                machine_no = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                if (machine.RegEvent(machine_no, 65535))//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                {
                    machine.OnFinger += new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
                    machine.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
                    machine.OnAttTransactionEx += new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
                    machine.OnFingerFeature += new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
                    machine.OnEnrollFingerEx += new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(axCZKEM1_OnEnrollFingerEx);
                    machine.OnDeleteTemplate += new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(axCZKEM1_OnDeleteTemplate);
                    machine.OnNewUser += new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(axCZKEM1_OnNewUser);
                    machine.OnHIDNum += new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(axCZKEM1_OnHIDNum);
                    machine.OnAlarm += new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(axCZKEM1_OnAlarm);
                    machine.OnDoor += new zkemkeeper._IZKEMEvents_OnDoorEventHandler(axCZKEM1_OnDoor);
                    machine.OnWriteCard += new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(axCZKEM1_OnWriteCard);
                    machine.OnEmptyCard += new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(axCZKEM1_OnEmptyCard);
                }


        }

        #endregion

        #region RealTime Events

        //When you place your finger on sensor of the device,this event will be triggered
        private void axCZKEM1_OnFinger()
        {
            Console.WriteLine("RTEvent OnFinger Has been Triggered");

        }

        //After you have placed your finger on the sensor(or swipe your card to the device),this event will be triggered.
        //If you passes the verification,the returned value userid will be the user enrollnumber,or else the value will be -1;
        private void axCZKEM1_OnVerify(int iUserID)
        {
            Console.WriteLine("RTEvent OnVerify Has been Triggered,Verifying...");
            if (iUserID != -1)
            {
                Console.WriteLine("Verified OK,the UserID is " + iUserID.ToString());
            }
            else
            {
                Console.WriteLine("Verified Failed... ");
            }

        }

        //If your fingerprint(or your card) passes the verification,this event will be triggered
        private void axCZKEM1_OnAttTransactionEx(string sEnrollNumber, int iIsInValid, int iAttState, int iVerifyMethod, int iYear, int iMonth, int iDay, int iHour, int iMinute, int iSecond, int iWorkCode)
        {
            string state = iAttState == 0 ? "Check in" : "Check out";
            SendData("{"+ $"\n \"machineNumber \": \"{machine_no}\",\n\"UserID\": {sEnrollNumber},\n\"attState\":\"{state}\",\n\"VerifyMethod\": {iVerifyMethod} , \n\"Time\": \"{iDay}-{iMonth}-{iYear}  {iHour}:{iMinute}:{iSecond}\""+ "}");

            /*
             {
            "machineNumber":"0"
            "UserID":1,
            "VerifyMethod":1,
            "Time":"// :"
            
            }
             */



        }

        //When you have enrolled your finger,this event will be triggered and return the quality of the fingerprint you have enrolled
        private void axCZKEM1_OnFingerFeature(int iScore)
        {
            if (iScore < 0)
            {
                Console.WriteLine("The quality of your fingerprint is poor");
            }
            else
            {
                Console.WriteLine("RTEvent OnFingerFeature Has been Triggered...Score:　" + iScore.ToString());
            }
        }

        //When you are enrolling your finger,this event will be triggered.(The event can only be triggered by TFT screen devices)
        private void axCZKEM1_OnEnrollFingerEx(string sEnrollNumber, int iFingerIndex, int iActionResult, int iTemplateLength)
        {
            if (iActionResult == 0)
            {
                Console.WriteLine("RTEvent OnEnrollFigerEx Has been Triggered....");
                Console.WriteLine(".....UserID: " + sEnrollNumber + " Index: " + iFingerIndex.ToString() + " tmpLen: " + iTemplateLength.ToString());
            }
            else
            {
                Console.WriteLine("RTEvent OnEnrollFigerEx Has been Triggered Error,actionResult=" + iActionResult.ToString());
            }
        }

        //When you have deleted one one fingerprint template,this event will be triggered.
        private void axCZKEM1_OnDeleteTemplate(int iEnrollNumber, int iFingerIndex)
        {
            Console.WriteLine("RTEvent OnDeleteTemplate Has been Triggered...");
            Console.WriteLine("...UserID=" + iEnrollNumber.ToString() + " FingerIndex=" + iFingerIndex.ToString());
        }

        //When you have enrolled a new user,this event will be triggered.
        private void axCZKEM1_OnNewUser(int iEnrollNumber)
        {
            Console.WriteLine("RTEvent OnNewUser Has been Triggered...");
            Console.WriteLine("...NewUserID=" + iEnrollNumber.ToString());
        }

        //When you swipe a card to the device, this event will be triggered to show you the card number.
        private void axCZKEM1_OnHIDNum(int iCardNumber)
        {
            Console.WriteLine("RTEvent OnHIDNum Has been Triggered...");
            Console.WriteLine("...Cardnumber=" + iCardNumber.ToString());
        }

        //When the dismantling machine or duress alarm occurs, trigger this event.
        private void axCZKEM1_OnAlarm(int iAlarmType, int iEnrollNumber, int iVerified)
        {
            Console.WriteLine("RTEvnet OnAlarm Has been Triggered...");
            Console.WriteLine("...AlarmType=" + iAlarmType.ToString());
            Console.WriteLine("...EnrollNumber=" + iEnrollNumber.ToString());
            Console.WriteLine("...Verified=" + iVerified.ToString());
        }

        //Door sensor event
        private void axCZKEM1_OnDoor(int iEventType)
        {
            Console.WriteLine("RTEvent Ondoor Has been Triggered...");
            Console.WriteLine("...EventType=" + iEventType.ToString());
        }

        //When you have emptyed the Mifare card,this event will be triggered.
        private void axCZKEM1_OnEmptyCard(int iActionResult)
        {
            Console.WriteLine("RTEvent OnEmptyCard Has been Triggered...");
            if (iActionResult == 0)
            {
                Console.WriteLine("...Empty Mifare Card OK");
            }
            else
            {
                Console.WriteLine("...Empty Failed");
            }
        }

        //When you have written into the Mifare card ,this event will be triggered.
        private void axCZKEM1_OnWriteCard(int iEnrollNumber, int iActionResult, int iLength)
        {
            Console.WriteLine("RTEvent OnWriteCard Has been Triggered...");
            if (iActionResult == 0)
            {
                Console.WriteLine("...Write Mifare Card OK");
                Console.WriteLine("...EnrollNumber=" + iEnrollNumber.ToString());
                Console.WriteLine("...TmpLength=" + iLength.ToString());
            }
            else
            {
                Console.WriteLine("...Write Failed");
            }
        }





        #endregion



        static void SendData(string data)
        {

            client = new TcpClient();
            client.Connect(IPAddress.Loopback, 3000);
            NetworkStream Ns = client.GetStream();
            byte[] bytesToSend = Encoding.ASCII.GetBytes(data);
            Ns.Write(bytesToSend, 0, bytesToSend.Length);
                
        }
        #endregion

        #region trial region






        #endregion
    }
} 
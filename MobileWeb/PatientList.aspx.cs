﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using WebForms;
using OpenDentBusiness;
using OpenDentBusiness.Mobile;

namespace MobileWeb {
	public partial class PatientList:System.Web.UI.Page {
		private long CustomerNum=0;
		private string searchterm="";
		List<Patientm> patientmList=new List<Patientm>();
		
		protected void Page_Load(object sender,EventArgs e) {
			try {
				if(!SetCustomerNum()) {
						return;
				}
				//Thread.Sleep(1500);
				if(Request["searchterm"]!=null) {
					searchterm=Request["searchterm"].Trim();
				}
				if(searchterm!="") {
					patientmList=Patientms.GetPatientms(CustomerNum,searchterm);
				}
				if(patientmList.Count==0) {
					MessageNoPatients.Text="No patients found. Please search again";
				}
				Repeater1.DataSource=patientmList;
				Repeater1.DataBind();
			}
			catch(Exception ex) {
				Logger.LogError(ex);
			}
		}

		private bool SetCustomerNum() {
			Message.Text="";
			if(Session["CustomerNum"]==null) {
				return false;
			}
			Int64.TryParse(Session["CustomerNum"].ToString(),out CustomerNum);
			if(CustomerNum!=0) {
				Message.Text="LoggedIn";
			}
			return true;
		}
	}
}
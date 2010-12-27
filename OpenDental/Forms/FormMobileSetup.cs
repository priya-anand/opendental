using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDentBusiness.Mobile;

namespace OpenDental {
	public partial class FormMobileSetup:Form {
		static string RegistrationKey;
		static MobileWeb.Mobile mb = new MobileWeb.Mobile();
		static DateTime MobileSyncDateTimeLastRun;
		static string MobileSyncServerURL;
		static int MobileSyncIntervalMinutes;
		static DateTime MobileExcludeApptsBeforeDate;

		public FormMobileSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMobileSetup_Load(object sender,EventArgs e) {
			InitializeVariables();
			textDateTimeLastRun.Text=MobileSyncDateTimeLastRun.ToShortDateString()+" "+MobileSyncDateTimeLastRun.ToShortTimeString();
			textboxMobileSyncServerURL.Text=MobileSyncServerURL;
			textBoxSynchMinutes.Text=MobileSyncIntervalMinutes+"";
			textDateBefore.Text=MobileExcludeApptsBeforeDate.ToShortDateString();
			butSavePreferences.Enabled=false;
		}

		private void InitializeVariables() {
			RegistrationKey=PrefC.GetString(PrefName.RegistrationKey);
			MobileSyncServerURL=PrefC.GetString(PrefName.MobileSyncServerURL);
			MobileSyncDateTimeLastRun=PrefC.GetDateT(PrefName.MobileSyncDateTimeLastRun);
			MobileSyncIntervalMinutes=PrefC.GetInt(PrefName.MobileSyncIntervalMinutes);
			MobileExcludeApptsBeforeDate=PrefC.GetDateT(PrefName.MobileExcludeApptsBeforeDate);
		}

		public void Synch(DateTime GetChangedSince) {
			try {
				#if DEBUG
					IgnoreCertificateErrors();// used with faulty certificates only while debugging.
				#endif
					if(mb.GetCustomerNum(RegistrationKey)==0) {
					MsgBox.Show(this,"Registration key provided by the dental office is incorrect");
					return;
				}
				//CreatePatients(100000);
				//CreateAppointments(10); // for each patient
				DateTime MobileSyncDateTimeLastRunNew= MiscData.GetNowDateTime();
				long[] patNumArray=Patientms.GetChangedSincePatNums(GetChangedSince);

				SynchPatients(new List<long>(patNumArray));
				long mem2=GC.GetTotalMemory(false);
				//MessageBox.Show("M1 Memory in MB=" + (mem1-mem2)/1000000);
				// major problem with system out of mem exception for a large number of recors
				//mb.SynchPatients(RegistrationKey,ChangedPatientmList.ToArray());
				//List<Appointmentm> ChangedAppointmentmList=Appointmentms.GetChanged(GetChangedSince,MobileExcludeApptsBeforeDate);
				//mb.SynchAppointments(RegistrationKey,ChangedAppointmentmList.ToArray());
				Prefs.UpdateDateT(PrefName.MobileSyncDateTimeLastRun,MobileSyncDateTimeLastRunNew);
				MobileSyncDateTimeLastRun=MobileSyncDateTimeLastRunNew;
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
		public void SynchPatients(List<long> patNumList) {
			int BlockLength =10;
			for(int start=0;start<patNumList.Count;start+=BlockLength) {
				if((start+BlockLength)>patNumList.Count) {
					BlockLength=patNumList.Count-start;
				}
				List<long> BlockPatNumList=patNumList.GetRange(start,BlockLength);
				List<Patientm> ChangedPatientmList=Patientms.GetMultPats(BlockPatNumList);
				mb.SynchPatients(RegistrationKey,ChangedPatientmList.ToArray());
			}
		}

		public void Synch() {
			Synch(MobileSyncDateTimeLastRun);
		}

		public void SynchFull() {
			DateTime FullSynchDateTime=new DateTime(1880,1,1);
			Synch(FullSynchDateTime);
		}
					
		/// <summary>
		/// An empty method to test if the webservice is up and running. This was made with the intention of testing the correctness of the webservice URL. If an incorrect webservice URL is used in a background thread the exception cannot be handled easily to a point where even a correct URL cannot be keyed in by the user. Because an exception in a background thread closes the Form which spawned it.
		/// </summary>
		/// <returns></returns>
		private bool TestWebServiceExists() {
			try {
				mb.Url=MobileSyncServerURL;
				if(mb.ServiceExists()) {
					return true;
				}
			}
			catch {//(Exception ex) {
				return false;
			}
			return true;
		}

		/// <summary>
		///  This method is used only for testing with security certificates that has problems.
		/// </summary>
		private void IgnoreCertificateErrors() {
			///the line below will allow the code to continue by not throwing an exception.
			///It will accept the security certificate if there is a problem with the security certificate.
			System.Net.ServicePointManager.ServerCertificateValidationCallback+=
			delegate(object sender,System.Security.Cryptography.X509Certificates.X509Certificate certificate,
									System.Security.Cryptography.X509Certificates.X509Chain chain,
									System.Net.Security.SslPolicyErrors sslPolicyErrors) {
				return true;
			};
		}

		private void timerRefreshLastSynchTime_Tick(object sender,EventArgs e) {
			textDateTimeLastRun.Text=MobileSyncDateTimeLastRun.ToShortDateString()+" "+MobileSyncDateTimeLastRun.ToShortTimeString();
		}

		private void butSavePreferences_Click(object sender,EventArgs e) {
			Prefs.UpdateString(PrefName.MobileSyncServerURL,textboxMobileSyncServerURL.Text.Trim());
			MobileSyncServerURL=textboxMobileSyncServerURL.Text.Trim();
			if(!FieldsValid()) {
				return;
			}
			Prefs.UpdateInt(PrefName.MobileSyncIntervalMinutes,PIn.Int(textBoxSynchMinutes.Text));
			MobileSyncIntervalMinutes=PIn.Int(textBoxSynchMinutes.Text);
			SetMobileExcludeApptsBeforeDate();
			butSavePreferences.Enabled=false;
		}

		private void textboxMobileSyncServerURL_TextChanged(object sender,EventArgs e) {
			butSavePreferences.Enabled=true;
		}

		private void textBoxSynchMinutes_TextChanged(object sender,EventArgs e) {
			butSavePreferences.Enabled=true;
		}

		private bool FieldsValid() {
			if(textDateBefore.errorProvider1.GetError(textDateBefore)!=""
				||textBoxSynchMinutes.errorProvider1.GetError(textBoxSynchMinutes)!="") {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			return true;
	}
		
		/// <summary>
		/// For testing only
		/// </summary>
		private void CreatePatients(int PatientCount) {
			for(int i=0;i<PatientCount;i++) {
				Patient newPat=new Patient();
				newPat.LName="Mathew"+i;
				newPat.FName="Dennis"+i;
				newPat.Address="Address Line 1.Address Line 1___"+i;
				newPat.Address2="Address Line 2. Address Line 2__"+i;
				newPat.AddrNote="Lives off in far off Siberia Lives off in far off Siberia"+i;
				newPat.AdmitDate=new DateTime(1985,3,3).AddDays(i);
				newPat.ApptModNote="Flies from Siberia on specially chartered flight piloted by goblins:)"+i;
				newPat.AskToArriveEarly=1555;
				newPat.BillingType=3;
				newPat.ChartNumber="111111"+i;
				newPat.City="NL";
				newPat.ClinicNum=i;
				newPat.County= "county"+i;
				newPat.CreditType="A";
				newPat.DateFirstVisit=new DateTime(1985,3,3).AddDays(i);
				newPat.Email="dennis.mathew________________seb@siberiacrawlmail.com";
				newPat.HmPhone="416-222-5678";
				newPat.WkPhone="416-222-5678";
				newPat.Zip="M3L2L9";
				newPat.WirelessPhone="416-222-5678";
				newPat.Birthdate=new DateTime(1970,3,3).AddDays(i);
				Patients.Insert(newPat,false);
				//set Guarantor field the same as PatNum
				Patient patOld=newPat.Copy();
				newPat.Guarantor=newPat.PatNum;
				Patients.Update(newPat,patOld);
			}
		}

		/// <summary>
		/// For testing only
		/// </summary>
		private void CreateAppointments(int AppointmentCount) {
			long[] patNumArray=Patients.GetAllPatNums();
			DateTime appdate= new DateTime(2010,12,1,11,0,0);
			for(int i=0;i<patNumArray.Length;i++) {
				appdate=appdate.AddDays(2);
				for(int j=0;j<AppointmentCount;j++) {
					Appointment apt=new Appointment();
					apt.PatNum=patNumArray[i];
					apt.DateTimeArrived=appdate;
					apt.DateTimeAskedToArrive=appdate;
					apt.DateTimeDismissed=appdate;
					apt.DateTimeSeated=appdate;
					apt.Note="some notenote noten otenotenot enotenot enote"+j;
					apt.IsNewPatient=true;
					apt.ProvNum=3;
					apt.AptStatus=ApptStatus.Scheduled;
					apt.AptDateTime=appdate;
					Appointments.Insert(apt);
				}
			}
		}

		/// <summary>
		/// If the MobileExcludeApptsBeforeDate is not specified then it defaults to a year before the current time.
		/// </summary>
		private void SetMobileExcludeApptsBeforeDate() {
			if(textDateBefore.Text.Trim()=="") {
				MobileExcludeApptsBeforeDate=DateTime.Now.AddYears(-1);
			}
			else {
				Prefs.UpdateDateT(PrefName.MobileExcludeApptsBeforeDate,PIn.Date(textDateBefore.Text));
				MobileExcludeApptsBeforeDate=PIn.Date(textDateBefore.Text);
			}
		}

		private void butSync_Click(object sender,EventArgs e) {
			if(!TestWebServiceExists()) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Either the web service is not available or the WebHostSynch URL is incorrect");
				return;
			}
			if(!FieldsValid()) {
				return;
			}
			if(MobileSyncIntervalMinutes==0) {
				// Charge the customer!
				MsgBox.Show(this,"You must be a paid customer to use this feature");
				//return;
			}
			if(MobileSyncDateTimeLastRun.Year<1880) {
				MsgBox.Show(this,"Sync has never been run.  You must do a full sync first.");
				return;
			}
			SetMobileExcludeApptsBeforeDate();
			Cursor=Cursors.WaitCursor;
			try {
				Synch();
				textDateTimeLastRun.Text=MobileSyncDateTimeLastRun.ToShortDateString()+" "+MobileSyncDateTimeLastRun.ToShortTimeString();
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
			}
			Cursor=Cursors.Default;
		}

		private void butFullSync_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,true,"This will be time consuming.  Continue anyway?")) {
				return;
			}
			SetMobileExcludeApptsBeforeDate();
			Cursor=Cursors.WaitCursor;
			try {
				SynchFull();
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
			}
			Cursor=Cursors.Default;
			/*if(objCount==0) {
				MsgBox.Show(this,"Done. No sync necessary.");
			}
			else {
				MessageBox.Show(Lan.g(this,"Done.  Objects exported: ")+objCount.ToString());
			}*/
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}










	}
}
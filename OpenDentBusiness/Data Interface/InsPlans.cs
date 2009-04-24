using CodeBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace OpenDentBusiness {
	///<summary></summary>
	public class InsPlans {
		///<summary>Also fills PlanNum from db.</summary>
		public static void Insert(InsPlan plan) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),plan);
				return;
			}
			if(PrefC.RandomKeys) {
				plan.PlanNum=MiscData.GetKey("insplan","PlanNum");
			}
			string command= "INSERT INTO insplan (";
			if(PrefC.RandomKeys) {
				command+="PlanNum,";
			}
			command+="Subscriber,"
				+"DateEffective,DateTerm,GroupName,GroupNum,PlanNote,"
				+"FeeSched,ReleaseInfo,AssignBen,PlanType,ClaimFormNum,UseAltCode,"
				+"ClaimsUseUCR,CopayFeeSched,SubscriberID,"
				+"EmployerNum,CarrierNum,AllowedFeeSched,TrojanID,DivisionNo,BenefitNotes,IsMedical,SubscNote,FilingCode,"
				+"DentaideCardSequence,ShowBaseUnits,DedBeforePerc,CodeSubstNone,IsHidden) VALUES(";
			if(PrefC.RandomKeys) {
				command+="'"+POut.PInt(plan.PlanNum)+"', ";
			}
			command+=
				 "'"+POut.PInt(plan.Subscriber)+"', "
				+POut.PDate(plan.DateEffective)+", "
				+POut.PDate(plan.DateTerm)+", "
				+"'"+POut.PString(plan.GroupName)+"', "
				+"'"+POut.PString(plan.GroupNum)+"', "
				+"'"+POut.PString(plan.PlanNote)+"', "
				+"'"+POut.PInt(plan.FeeSched)+"', "
				+"'"+POut.PBool(plan.ReleaseInfo)+"', "
				+"'"+POut.PBool(plan.AssignBen)+"', "
				+"'"+POut.PString(plan.PlanType)+"', "
				+"'"+POut.PInt(plan.ClaimFormNum)+"', "
				+"'"+POut.PBool(plan.UseAltCode)+"', "
				+"'"+POut.PBool(plan.ClaimsUseUCR)+"', "
				+"'"+POut.PInt(plan.CopayFeeSched)+"', "
				+"'"+POut.PString(plan.SubscriberID)+"', "
				+"'"+POut.PInt(plan.EmployerNum)+"', "
				+"'"+POut.PInt(plan.CarrierNum)+"', "
				+"'"+POut.PInt(plan.AllowedFeeSched)+"', "
				+"'"+POut.PString(plan.TrojanID)+"', "
				+"'"+POut.PString(plan.DivisionNo)+"', "
				+"'"+POut.PString(plan.BenefitNotes)+"', "
				+"'"+POut.PBool(plan.IsMedical)+"', "
				+"'"+POut.PString(plan.SubscNote)+"', "
				+"'"+POut.PInt((int)plan.FilingCode)+"', "
				+"'"+POut.PInt((int)plan.DentaideCardSequence)+"', "
				+"'"+POut.PBool(plan.ShowBaseUnits)+"', "
				+"'"+POut.PBool(plan.DedBeforePerc)+"', "
				+"'"+POut.PBool(plan.CodeSubstNone)+"', "
				+"'"+POut.PBool(plan.IsHidden)+"')";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				plan.PlanNum=Db.NonQ(command,true);
			}
		}

		///<summary></summary>
		public static void Update(InsPlan plan) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),plan);
				return;
			}
			string command="UPDATE insplan SET "
				+"Subscriber = '"    +POut.PInt   (plan.Subscriber)+"'"
				+",DateEffective = "+POut.PDate  (plan.DateEffective)
				+",DateTerm = "     +POut.PDate  (plan.DateTerm)
				+",GroupName = '"    +POut.PString(plan.GroupName)+"'"
				+",GroupNum = '"     +POut.PString(plan.GroupNum)+"'"
				+",PlanNote = '"     +POut.PString(plan.PlanNote)+"'"
				+",FeeSched = '"     +POut.PInt   (plan.FeeSched)+"'"
				+",ReleaseInfo = '"  +POut.PBool  (plan.ReleaseInfo)+"'"
				+",AssignBen = '"    +POut.PBool  (plan.AssignBen)+"'"
				+",PlanType = '"     +POut.PString(plan.PlanType)+"'"
				+",ClaimFormNum = '" +POut.PInt   (plan.ClaimFormNum)+"'"
				+",UseAltcode = '"   +POut.PBool  (plan.UseAltCode)+"'"
				+",ClaimsUseUCR = '" +POut.PBool  (plan.ClaimsUseUCR)+"'"
				+",CopayFeeSched = '"+POut.PInt   (plan.CopayFeeSched)+"'"
				+",SubscriberID = '" +POut.PString(plan.SubscriberID)+"'"
				+",EmployerNum = '"  +POut.PInt   (plan.EmployerNum)+"'"
				+",CarrierNum = '"   +POut.PInt   (plan.CarrierNum)+"'"
				+",AllowedFeeSched='"+POut.PInt   (plan.AllowedFeeSched)+"'"
				+",TrojanID='"       +POut.PString(plan.TrojanID)+"'"
				+",DivisionNo='"     +POut.PString(plan.DivisionNo)+"'"
				+",BenefitNotes='"   +POut.PString(plan.BenefitNotes)+"'"
				+",IsMedical='"      +POut.PBool  (plan.IsMedical)+"'"
				+",SubscNote='"      +POut.PString(plan.SubscNote)+"'"
				+",FilingCode='"     +POut.PInt((int)plan.FilingCode)+"'"
				+",DentaideCardSequence='" +POut.PInt(plan.DentaideCardSequence)+"'"
				+",ShowBaseUnits='"  +POut.PBool(plan.ShowBaseUnits)+"'"
				+",DedBeforePerc='"  +POut.PBool(plan.DedBeforePerc)+"'"
				+",CodeSubstNone='"  +POut.PBool(plan.CodeSubstNone)+"'"
				+",IsHidden='"       +POut.PBool(plan.IsHidden)+"'"
				+" WHERE PlanNum = '"+POut.PInt   (plan.PlanNum)+"'";
			Db.NonQ(command);
		}

		///<summary>Called from FormInsPlan when applying changes to all identical insurance plans. This updates the synchronized fields for all plans like the specified insPlan.  Current InsPlan must be set to the new values that we want.  BenefitNotes and SubscNote are specific to subscriber and are not changed.  PlanNotes are handled separately in a different function after this one is complete.</summary>
		public static void UpdateForLike(InsPlan like, InsPlan plan) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),like,plan);
				return;
			}
			string command= "UPDATE insplan SET "
				+"EmployerNum = '"     +POut.PInt   (plan.EmployerNum)+"'"
				+",GroupName = '"      +POut.PString(plan.GroupName)+"'"
				+",GroupNum = '"       +POut.PString(plan.GroupNum)+"'"
				+",DivisionNo = '"     +POut.PString(plan.DivisionNo)+"'"
				+",CarrierNum = '"     +POut.PInt   (plan.CarrierNum)+"'"
				+",PlanType = '"       +POut.PString(plan.PlanType)+"'"
				+",UseAltCode = '"     +POut.PBool  (plan.UseAltCode)+"'"
				+",IsMedical = '"      +POut.PBool  (plan.IsMedical)+"'"
				+",ClaimsUseUCR = '"   +POut.PBool  (plan.ClaimsUseUCR)+"'"
				+",FeeSched = '"       +POut.PInt   (plan.FeeSched)+"'"
				+",CopayFeeSched = '"  +POut.PInt   (plan.CopayFeeSched)+"'"
				+",ClaimFormNum = '"   +POut.PInt   (plan.ClaimFormNum)+"'"
				+",AllowedFeeSched= '" +POut.PInt   (plan.AllowedFeeSched)+"'"
				+",TrojanID = '"       +POut.PString(plan.TrojanID)+"'"
				+",FilingCode = '"     +POut.PInt   ((int)plan.FilingCode)+"'"
				+",ShowBaseUnits = '"  +POut.PBool  (plan.ShowBaseUnits)+"'"
				+",ShowBaseUnits = '"  +POut.PBool  (plan.DedBeforePerc)+"'"
				+",CodeSubstNone='"    +POut.PBool  (plan.CodeSubstNone)+"'"
				+",IsHidden='"         +POut.PBool  (plan.IsHidden)+"'"
				+" WHERE "
				+"EmployerNum = '"        +POut.PInt   (like.EmployerNum)+"' "
				+"AND GroupName = '"      +POut.PString(like.GroupName)+"' "
				+"AND GroupNum = '"       +POut.PString(like.GroupNum)+"' "
				+"AND DivisionNo = '"     +POut.PString(like.DivisionNo)+"'"
				+"AND CarrierNum = '"     +POut.PInt   (like.CarrierNum)+"' "
				+"AND IsMedical = '"      +POut.PBool  (like.IsMedical)+"'";
			Db.NonQ(command);
		}

		///<summary>It's fastest if you supply a plan list that contains the plan, but it also works just fine if it can't initally locate the plan in the list.  You can supply an array of length 0.  If still not found, returns null.</summary>
		public static InsPlan GetPlan(int planNum,List<InsPlan> planList) {
			//No need to check RemotingRole; no call to db.
			InsPlan retPlan=new InsPlan();
			if(planNum==0) {
				return null;
			}
			if(planList==null) {
				planList=new List<InsPlan>();
			}
			bool found=false;
			for(int i=0;i<planList.Count;i++) {
				if(planList[i].PlanNum==planNum) {
					found=true;
					retPlan=planList[i];
				}
			}
			if(!found) {
				retPlan=Refresh(planNum);//retPlan will now be null if not found
			}
			if(retPlan==null) {
				//MessageBox.Show(Lan.g("InsPlans","Database is inconsistent.  Please run the database maintenance tool."));
				return new InsPlan();
			}
			if(retPlan==null) {
				return null;
			}
			return retPlan;
		}

		/*
		///<summary>Will return null if no active plan for that ordinal.  Ordinal means primary, secondary, etc.</summary>
		public static InsPlan GetPlanByOrdinal(int patNum,int ordinal) {
			string command="SELECT * FROM insplan WHERE EXISTS "
				+"(SELECT * FROM patplan WHERE insplan.PlanNum=patplan.PlanNum "
				+"AND patplan.PatNum="+POut.PInt(patNum)
				+" AND patplan.Ordinal="+POut.PInt(ordinal);
			//num = '"+planNum+"'";
		}*/

		public static InsPlan[] GetByTrojanID(string trojanID) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<InsPlan[]>(MethodBase.GetCurrentMethod(),trojanID);
			} 
			string command="SELECT * FROM insplan WHERE TrojanID = '"+POut.PString(trojanID)+"'";
			DataTable table=Db.GetTable(command);
			return RefreshFill(table).ToArray();
		}

		///<summary>Used in FormInsSelectSubscr to get a list of insplans for one subscriber directly from the database.</summary>
		public static List <InsPlan> GetListForSubscriber(int subscriber) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return MiscUtils.ArrayToList(Meth.GetObject<InsPlan[]>(MethodBase.GetCurrentMethod(),subscriber));
			} 
			string command="SELECT * FROM insplan WHERE Subscriber="+POut.PInt(subscriber);
			DataTable table=Db.GetTable(command);
			return RefreshFill(table);
		}

		///<summary>Only loads one plan from db. Can return null.</summary>
		private static InsPlan Refresh(int planNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<InsPlan>(MethodBase.GetCurrentMethod(),planNum);
			} 
			if(planNum==0)
				return null;
			string command="SELECT * FROM insplan WHERE plannum = '"+planNum+"'";
			DataTable table=Db.GetTable(command);
			List<InsPlan> planList=RefreshFill(table);
			if(planList.Count>0) {
				return planList[0].Copy();
			}
			else {
				return null;
			}
		}

		///<summary>Gets new List for the specified family.  The only plans it misses are for claims with no current coverage.  These are handled as needed.</summary>
		public static List<InsPlan> Refresh(Family Fam) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<InsPlan>>(MethodBase.GetCurrentMethod(),Fam);
			} 
			string command=
				"(SELECT * from insplan "
				+"WHERE";
			//subscribers in family
			for(int i=0;i<Fam.ListPats.Length;i++) {
				if(i>0) {
					command+=" OR";
				}
				command+=" Subscriber="+POut.PInt(Fam.ListPats[i].PatNum);
			}
			//in union, distinct is implied
			command+=") UNION (SELECT insplan.* FROM insplan,patplan WHERE insplan.PlanNum=patplan.PlanNum AND (";
			for(int i=0;i<Fam.ListPats.Length;i++) {
				if(i>0) {
					command+=" OR";
				}
				command+=" patplan.PatNum="+POut.PInt(Fam.ListPats[i].PatNum);
			}
			//command+=")) ORDER BY DateEffective";//FIXME:UNION-ORDER-BY
			command+=")) ORDER BY 3";//***ORACLE ORDINAL
			//Debug.WriteLine(command);
			DataTable table=Db.GetTable(command);
			return RefreshFill(table);
		}

		private static List<InsPlan> RefreshFill(DataTable table) {
			List<InsPlan> planList=new List<InsPlan>();
			InsPlan plan;
			for(int i=0;i<table.Rows.Count;i++) {
				plan=new InsPlan();
				plan.PlanNum        = PIn.PInt   (table.Rows[i][0].ToString());
				plan.Subscriber     = PIn.PInt   (table.Rows[i][1].ToString());
				plan.DateEffective  = PIn.PDate  (table.Rows[i][2].ToString());
				plan.DateTerm       = PIn.PDate  (table.Rows[i][3].ToString());
				plan.GroupName      = PIn.PString(table.Rows[i][4].ToString());
				plan.GroupNum       = PIn.PString(table.Rows[i][5].ToString());
				plan.PlanNote       = PIn.PString(table.Rows[i][6].ToString());
				plan.FeeSched       = PIn.PInt   (table.Rows[i][7].ToString());
				plan.ReleaseInfo    = PIn.PBool  (table.Rows[i][8].ToString());
				plan.AssignBen      = PIn.PBool  (table.Rows[i][9].ToString());
				plan.PlanType       = PIn.PString(table.Rows[i][10].ToString());
				plan.ClaimFormNum   = PIn.PInt   (table.Rows[i][11].ToString());
				plan.UseAltCode     = PIn.PBool  (table.Rows[i][12].ToString());
				plan.ClaimsUseUCR   = PIn.PBool  (table.Rows[i][13].ToString());
				plan.CopayFeeSched  = PIn.PInt   (table.Rows[i][14].ToString());
				plan.SubscriberID   = PIn.PString(table.Rows[i][15].ToString());
				plan.EmployerNum    = PIn.PInt   (table.Rows[i][16].ToString());
				plan.CarrierNum     = PIn.PInt   (table.Rows[i][17].ToString());
				plan.AllowedFeeSched= PIn.PInt   (table.Rows[i][18].ToString());
				plan.TrojanID       = PIn.PString(table.Rows[i][19].ToString());
				plan.DivisionNo     = PIn.PString(table.Rows[i][20].ToString());
				plan.BenefitNotes   = PIn.PString(table.Rows[i][21].ToString());
				plan.IsMedical      = PIn.PBool  (table.Rows[i][22].ToString());
				plan.SubscNote      = PIn.PString(table.Rows[i][23].ToString());
				plan.FilingCode     = (InsFilingCode)PIn.PInt(table.Rows[i][24].ToString());
				plan.DentaideCardSequence= PIn.PInt(table.Rows[i][25].ToString());
				plan.ShowBaseUnits  = PIn.PBool  (table.Rows[i][26].ToString());
				plan.DedBeforePerc  = PIn.PBool  (table.Rows[i][27].ToString());
				plan.CodeSubstNone  = PIn.PBool  (table.Rows[i][28].ToString());
				plan.IsHidden       = PIn.PBool  (table.Rows[i][29].ToString());
				planList.Add(plan);
			}
			return planList;
		}

		///<summary>Gets a description of the specified plan, including carrier name and subscriber. It's fastest if you supply a plan list that contains the plan, but it also works just fine if it can't initally locate the plan in the list.  You can supply an array of length 0 for both family and planlist.</summary>
		public static string GetDescript(int planNum,Family family,List<InsPlan> planList) {
			//No need to check RemotingRole; no call to db.
			if(planNum==0)
				return "";
			InsPlan plan=GetPlan(planNum,planList);
			if(plan==null || plan.PlanNum==0) {
				return "";
			}
			string subscriber=family.GetNameInFamFL(plan.Subscriber);
			if(subscriber=="") {//subscriber from another family
				subscriber=Patients.GetLim(plan.Subscriber).GetNameLF();
			}
			string retStr="";
			//loop just to get the index of the plan in the family list
			bool otherFam=true;
			for(int i=0;i<planList.Count;i++) {
				if(planList[i].PlanNum==planNum) {
					otherFam=false;
					//retStr += (i+1).ToString()+": ";
				}
			}
			if(otherFam)//retStr=="")
				retStr="(other fam):";
			Carrier carrier=Carriers.GetCarrier(plan.CarrierNum);
			string carrierName=carrier.CarrierName;
			if(carrierName.Length>20) {
				carrierName=carrierName.Substring(0,20)+"...";
			}
			retStr+=carrierName;
			retStr+=" ("+subscriber+")";
			return retStr;
		}

		///<summary>Used in Ins lines in Account module and in Family module.</summary>
		public static string GetCarrierName(int planNum,List<InsPlan> planList) {
			//No need to check RemotingRole; no call to db.
			InsPlan plan=GetPlan(planNum,planList);
			if(plan==null) {
				return "";
			}
			Carrier carrier=Carriers.GetCarrier(plan.CarrierNum);
			if(carrier.CarrierNum==0) {//if corrupted
				return "";
			}
			return carrier.CarrierName;
		}

		/// <summary>Only used once in Claims.cs.  Gets insurance benefits remaining for one benefit year.  Returns actual remaining insurance based on ClaimProc data, taking into account inspaid and ins pending. Must supply all claimprocs for the patient.  Date used to determine which benefit year to calc.  Usually today's date.  The insplan.PlanNum is the plan to get value for.  ExcludeClaim is the ClaimNum to exclude, or enter -1 to include all.  This does not yet handle calculations where ortho max is different from regular max.  Just takes the most general annual max, and subtracts all benefits used from all categories.</summary>
		public static double GetInsRem(ClaimProc[] ClaimProcList,DateTime date,int planNum,int patPlanNum,int excludeClaim,List<InsPlan> planList,List<Benefit> benList) {
			//No need to check RemotingRole; no call to db.
			double insUsed=GetInsUsed(ClaimProcList,date,planNum,patPlanNum,excludeClaim,planList,benList);
			InsPlan plan=InsPlans.GetPlan(planNum,planList);
			double insPending=GetPending(ClaimProcList,date,plan,patPlanNum,excludeClaim,benList);
			double annualMax=Benefits.GetAnnualMax(benList,planNum,patPlanNum);
			if(annualMax<0) {
				return 999999;
			}
			if(annualMax-insUsed-insPending<0) {
				return 0;
			}
			return annualMax-insUsed-insPending;
		}

		/// <summary>Get insurance benefits used for one benefit year.  Returns actual insurance used based on ClaimProc data. Must supply all claimprocs for the patient.  Must supply all benefits for patient so that we know if it's a service year or a calendar year.  asofDate is used to determine which benefit year to calc.  Usually date of service for a claim.  The insplan.PlanNum is the plan to get value for.  ExcludeClaim is the ClaimNum to exclude, or enter -1 to include all.</summary>
		public static double GetInsUsed(ClaimProc[] ClaimProcList,DateTime asofDate,int planNum,int patPlanNum,int excludeClaim,List<InsPlan> planList,List<Benefit> benList) {
			//No need to check RemotingRole; no call to db.
			InsPlan curPlan=GetPlan(planNum,planList);
			if(curPlan==null) {
				return 0;
			}
			//get the most recent renew date, possibly including today:
			DateTime renewDate=Benefits.GetRenewDate(benList,planNum,patPlanNum,curPlan.DateEffective,asofDate);
			//DateTime startDate;//for benefit year
			DateTime stopDate=renewDate.AddYears(1);
			/*
			//if renew date is earlier this year or is today(assuming typical situation of date being today)
			if(renewDate.Month <= date.Month && renewDate.Day <= date.Day) {
				startDate=new DateTime(date.Year,renewDate.Month,renewDate.Day);
				stopDate=new DateTime(date.Year+1,renewDate.Month,renewDate.Day);
			}
			else {//otherwise, renew date must be late last year
				startDate=new DateTime(date.Year-1,renewDate.Month,renewDate.Day);
				stopDate=new DateTime(date.Year,renewDate.Month,renewDate.Day);
			}*/
			double retVal=0;
			for(int i=0;i<ClaimProcList.Length;i++) {
				if(ClaimProcList[i].PlanNum==planNum
					&& ClaimProcList[i].ClaimNum != excludeClaim
					&& ClaimProcList[i].ProcDate < stopDate
					&& ClaimProcList[i].ProcDate >= renewDate
					//enum ClaimProcStatus{NotReceived,Received,Preauth,Adjustment,Supplemental}
					&& ClaimProcList[i].Status!=ClaimProcStatus.Preauth) {
					if(ClaimProcList[i].Status==ClaimProcStatus.Received 
						|| ClaimProcList[i].Status==ClaimProcStatus.Adjustment
						|| ClaimProcList[i].Status==ClaimProcStatus.Supplemental) {
						retVal+=ClaimProcList[i].InsPayAmt;
					}
					else {//NotReceived
						//retVal-=ClaimProcList[i].InsPayEst;
					}
				}
			}
			return retVal;
		}

		///<summary>Get insurance deductible used for one benefit year.  Must supply all claimprocs for the patient.  Must supply all benefits for patient so that we know if it's a service year or a calendar year.  asofDate is used to determine which benefit year to calc.  Usually date of service for a claim.  The insplan.PlanNum is the plan to get value for.  ExcludeClaim is the ClaimNum to exclude, or enter -1 to include all.</summary>
		public static double GetDedUsed(ClaimProc[] ClaimProcList,DateTime asofDate,int planNum,int patPlanNum,int excludeClaim,List<InsPlan> planList,List<Benefit> benList) {
			//No need to check RemotingRole; no call to db.
			InsPlan curPlan=GetPlan(planNum,planList);
			if(curPlan==null) {
				return 0;
			}
			//get the most recent renew date, possibly including today. Date based on annual max.
			DateTime renewDate=Benefits.GetRenewDate(benList,planNum,patPlanNum,curPlan.DateEffective,asofDate);
			//DateTime startDate;//for benefit year
			DateTime stopDate=renewDate.AddYears(1);
			/*if(renewDate.Month <= date.Month && renewDate.Day <= date.Day) {
				startDate=new DateTime(date.Year,renewDate.Month,renewDate.Day);
				stopDate=new DateTime(date.Year+1,renewDate.Month,renewDate.Day);
			}
			else {//otherwise, renew date must be late last year
				startDate=new DateTime(date.Year-1,renewDate.Month,renewDate.Day);
				stopDate=new DateTime(date.Year,renewDate.Month,renewDate.Day);
			}*/
			double retVal=0;
			for(int i=0;i<ClaimProcList.Length;i++) {
				if(ClaimProcList[i].PlanNum==planNum
					&& ClaimProcList[i].ClaimNum != excludeClaim
					&& ClaimProcList[i].ProcDate < stopDate
					&& ClaimProcList[i].ProcDate >= renewDate
					//enum ClaimProcStatus{NotReceived,Received,Preauth,Adjustment,Supplemental}
					&& (ClaimProcList[i].Status==ClaimProcStatus.Adjustment
					|| ClaimProcList[i].Status==ClaimProcStatus.NotReceived
					|| ClaimProcList[i].Status==ClaimProcStatus.Received
					|| ClaimProcList[i].Status==ClaimProcStatus.Supplemental)
					)
				{
					retVal+=ClaimProcList[i].DedApplied;
				}
			}
			return retVal;
		}

		///<summary>Get pending insurance for a given plan for one benefit year. Include a ClaimProcList which is all claimProcs for the patient.  Must supply all benefits for patient so that we know if it's a service year or a calendar year.  asofDate used to determine which benefit year to calc.  Usually the date of service for a claim.  The insplan.PlanNum is the plan to get value for.</summary>
		public static double GetPending(ClaimProc[] ClaimProcList,DateTime asofDate,InsPlan curPlan,int patPlanNum,int excludeClaim,List<Benefit> benList) {
			//No need to check RemotingRole; no call to db.
			//InsPlan curPlan=GetPlan(planNum,PlanList);
			if(curPlan==null) {
				return 0;
			}
			//get the most recent renew date, possibly including today:
			DateTime renewDate=Benefits.GetRenewDate(benList,curPlan.PlanNum,patPlanNum,curPlan.DateEffective,asofDate);
			DateTime stopDate=renewDate.AddYears(1);
			double retVal=0;
			for(int i=0;i<ClaimProcList.Length;i++) {
				if(ClaimProcList[i].PlanNum==curPlan.PlanNum
					&& ClaimProcList[i].ClaimNum != excludeClaim
					&& ClaimProcList[i].ProcDate < stopDate
					&& ClaimProcList[i].ProcDate >= renewDate
					//enum ClaimProcStatus{NotReceived,Received,Preauth,Adjustment,Supplemental}
					&& ClaimProcList[i].Status==ClaimProcStatus.NotReceived)
				//Status Adjustment has no insPayEst, so can ignore it here.
				{
					retVal+=ClaimProcList[i].InsPayEst;
				}
			}
			return retVal;
		}

		///<summary>Used once from Claims and also in ContrTreat.  Gets insurance deductible remaining for one benefit year which includes the given date.  Must supply all claimprocs for the patient.  Must supply all benefits for patient so that we know if it's a service year or a calendar year.  Date used to determine which benefit year to calc.  Usually today's date.  The insplan.PlanNum is the plan to get value for.  ExcludeClaim is the ClaimNum to exclude, or enter -1 to include all.  The supplied procCode is needed because some deductibles, for instance, do not apply to preventive.</summary>
		public static double GetDedRem(ClaimProc[] ClaimProcList,DateTime date,int planNum,int patPlanNum,int excludeClaim,List <InsPlan> PlanList,List <Benefit> benList,string procCode){
			//No need to check RemotingRole; no call to db.
			double dedTot=Benefits.GetDeductibleByCode(benList,planNum,patPlanNum,procCode);
			double dedUsed=GetDedUsed(ClaimProcList,date,planNum,patPlanNum,excludeClaim,PlanList,benList);
			if(dedTot-dedUsed<0){
				return 0;
			}
			return dedTot-dedUsed;
		}

		/*
		///<Summary>Only used in TP to calculate discount for PPO procedure.  Will return -1 if no fee found.</Summary>
		public static double GetPPOAllowed(int codeNum,InsPlan plan){
			//plan has already been tested to not be null and to be a PPO plan.
			double fee=Fees.GetAmount(codeNum,plan.FeeSched);//could be -1
		}*/

		///<summary>This is used in FormQuery.SubmitQuery to allow display of carrier names.</summary>
		public static Hashtable GetHListAll(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Hashtable>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT insplan.PlanNum,carrier.CarrierName "
				+"FROM insplan,carrier "
				+"WHERE insplan.CarrierNum=carrier.CarrierNum";
			DataTable table=Db.GetTable(command);
			Hashtable HListAll=new Hashtable(table.Rows.Count);
			int plannum;
			string carrierName;
			for(int i=0;i<table.Rows.Count;i++){
				plannum=PIn.PInt(table.Rows[i][0].ToString());
				carrierName=PIn.PString(table.Rows[i][1].ToString());
				HListAll.Add(plannum,carrierName);
			}
			return HListAll;
		}

		///<summary>Gets all distinct notes for the planNums supplied.  Supply a planNum to exclude it.  Only called when closing FormInsPlan.  Includes blank notes.</summary>
		public static string[] GetNotesForPlans(List<int> planNums,int excludePlanNum){
			if(planNums.Count==0) {//this should never happen, but just in case...
				return new string[0];
			}
			if(planNums.Count==1 && planNums[0]==excludePlanNum){
				return new string[0];
			}
			string s="";
			for(int i=0;i<planNums.Count;i++) {
				if(planNums[i]==excludePlanNum){
					continue;
				}
				if(s!="") {
					s+=" OR";
				}
				s+=" PlanNum="+POut.PInt(planNums[i]);
			}
			string command="SELECT DISTINCT PlanNote FROM insplan WHERE"+s;
			DataTable table=Db.GetTable(command);
			string[] retVal=new string[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				retVal[i]=PIn.PString(table.Rows[i][0].ToString());
			}
			return retVal;
		}

		///<summary>Called when closing FormInsPlan to set the PlanNote for multiple plans at once.</summary>
		public static void UpdateNoteForPlans(List<int> planNums,string newNote){
			if(planNums.Count==0){
				return;
			}
			string s="";
			for(int i=0;i<planNums.Count;i++){
				if(i>0){
					s+=" OR";
				}
				s+=" PlanNum="+POut.PInt(planNums[i]);
			}
			string command="UPDATE insplan SET PlanNote='"+POut.PString(newNote)+"' "
				+"WHERE"+s;
			Db.NonQ(command);
		}

		///<summary>Called from FormInsPlan when user wants to view a benefit note for similar plans.  Should never include the current plan that the user is editing.  This function will get one note from the database, not including blank notes.  If no note can be found, then it returns empty string.</summary>
		public static string GetBenefitNotes(List<int> planNums){
			if(planNums.Count==0){
				return "";
			}
			string s="";
			for(int i=0;i<planNums.Count;i++) {
				if(i>0) {
					s+=" OR";
				}
				s+=" PlanNum="+POut.PInt(planNums[i]);
			}
			string command="SELECT BenefitNotes FROM insplan WHERE BenefitNotes != '' AND ("+s+") ";
			if(DataConnection.DBtype==DatabaseType.Oracle){
				command+="AND ROWNUM<=1";
			}else{//Assume MySQL
				command+="LIMIT 1";
			}
			DataTable table=Db.GetTable(command);
			//string[] retVal=new string[];
			if(table.Rows.Count==0){
				return "";
			}
			return PIn.PString(table.Rows[0][0].ToString());
		}

		///<summary>Only used once.  Gets a list of subscriber names from the database that have identical plan info as this one. Used to display in the insplan window.  The returned list never includes the plan that we're viewing.  Use excludePlan for this purpose; it's more consistent, because we have no way of knowing if the current plan will be picked up or not.</summary>
		public static string[] GetSubscribersForSamePlans(string employerName, string groupName, string groupNum,
				string divisionNo, string carrierName, bool isMedical, int excludePlan)
		{
			string command="SELECT CONCAT(CONCAT(LName,', '),FName) "
				+"FROM patient "
				+"LEFT JOIN insplan ON patient.PatNum=insplan.Subscriber "
				+"LEFT JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum "
				+"LEFT JOIN employer ON employer.EmployerNum = insplan.EmployerNum ";
			if(employerName==""){
				command+="WHERE employer.EmpName IS NULL ";
			}
			else{
				command+="WHERE employer.EmpName = '"+POut.PString(employerName)+"' ";
			}
			command+="AND insplan.GroupName = '"  +POut.PString(groupName)+"' "
				+"AND insplan.GroupNum = '"   +POut.PString(groupNum)+"' "
				+"AND insplan.DivisionNo = '" +POut.PString(divisionNo)+"' "
				+"AND carrier.CarrierName = '"+POut.PString(carrierName)+"' "
				+"AND insplan.IsMedical = '"  +POut.PBool(isMedical)+"' "
				+"AND insplan.PlanNum != "    +POut.PInt(excludePlan)
				+" ORDER BY LName,FName";
			DataTable table=Db.GetTable(command);
			string[] retStr=new string[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				retStr[i]=PIn.PString(table.Rows[i][0].ToString());
			}
			return retStr;
		}

		///<summary>Gets a list of PlanNums from the database of plans that have identical info as this one. Used to perform updates to benefits, etc.  Note that you have the option to include the current plan in the list.</summary>
		public static List<int> GetPlanNumsOfSamePlans(string employerName, string groupName, string groupNum,
				string divisionNo, string carrierName, bool isMedical, int planNum, bool includePlanNum) {
			string command="SELECT PlanNum FROM insplan "
				+"LEFT JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum "
				+"LEFT JOIN employer ON employer.EmployerNum = insplan.EmployerNum ";
			if(employerName==""){
				command+="WHERE employer.EmpName IS NULL ";
			}
			else{
				command+="WHERE employer.EmpName = '"+POut.PString(employerName)+"' ";
			}
			command+="AND insplan.GroupName = '"  +POut.PString(groupName)+"' "
				+"AND insplan.GroupNum = '"   +POut.PString(groupNum)+"' "
				+"AND insplan.DivisionNo = '" +POut.PString(divisionNo)+"' "
				+"AND carrier.CarrierName = '"+POut.PString(carrierName)+"' "
				+"AND insplan.IsMedical = '"  +POut.PBool  (isMedical)+"'"
				+"AND insplan.PlanNum != "+POut.PInt(planNum);
			DataTable table=Db.GetTable(command);
			List<int> retVal=new List<int>();
			//if(includePlanNum){
			//	retVal=new int[table.Rows.Count+1];
			//}
			//else{
			//	retVal=new int[table.Rows.Count];
			//}
			for(int i=0;i<table.Rows.Count;i++) {
				retVal.Add(PIn.PInt(table.Rows[i][0].ToString()));
			}
			if(includePlanNum){
				retVal.Add(planNum);
			}
			return retVal;
		}

		///<summary>Used from FormInsPlans to get a big list of many plans, organized by carrier name or by employer.  Identical plans are grouped as one row.</summary>
		public static DataTable GetBigList(bool byEmployer,string empName,string carrierName,string groupName,string groupNum,
			string trojanID,bool showHidden)
		{
			DataTable table=new DataTable();
			DataRow row;
			table.Columns.Add("Address");
			table.Columns.Add("City");
			table.Columns.Add("CarrierName");
			table.Columns.Add("ElectID");
			table.Columns.Add("EmpName");
			table.Columns.Add("GroupName");
			table.Columns.Add("GroupNum");
			table.Columns.Add("noSendElect");
			table.Columns.Add("Phone");
			table.Columns.Add("PlanNum");
			table.Columns.Add("plans");
			table.Columns.Add("State");
			table.Columns.Add("trojanID");
			table.Columns.Add("Zip");
			List<DataRow> rows=new List<DataRow>();
			string command="SELECT carrier.Address,carrier.City,CarrierName,ElectID,EmpName,GroupName,GroupNum,NoSendElect,"
				+"carrier.Phone,MAX(PlanNum) onePlanNum,"//for Oracle
				+"COUNT(*) plans,carrier.State,TrojanID,carrier.Zip, "
				+"CASE WHEN (EmpName IS NULL) THEN 1 ELSE 0 END as haveName "//for Oracle
				+"FROM insplan "
				+"LEFT JOIN employer ON employer.EmployerNum = insplan.EmployerNum "
				+"LEFT JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum "
				+"WHERE CarrierName LIKE '%"+POut.PString(carrierName)+"%' ";
			if(empName!="") {
				command+="AND EmpName LIKE '%"+POut.PString(empName)+"%' ";
			}
			if(groupName!="") {
				command+="AND GroupName LIKE '%"+POut.PString(groupName)+"%' ";
			}
			if(groupNum!="") {
				command+="AND GroupNum LIKE '%"+POut.PString(groupNum)+"%' ";
			}
			if(!showHidden){
				command+="AND insplan.IsHidden=0 ";
			}
			command+="GROUP BY insplan.EmployerNum,GroupName,GroupNum,DivisionNo,"
				+"insplan.CarrierNum,insplan.IsMedical,TrojanID ";
			if(DataConnection.DBtype==DatabaseType.Oracle){
				command+=",carrier.Address,carrier.City,CarrierName,ElectID,EmpName,NoSendElect,carrier.Phone,carrier.State,carrier.Zip ";
			}
			if(byEmployer) {
				command+="ORDER BY haveName,EmpName,CarrierName";
			}
			else {//not by employer
				command+="ORDER BY CarrierName";
			}
			DataTable rawT=Db.GetTable(command);
			for(int i=0;i<rawT.Rows.Count;i++) {
				row=table.NewRow();
				row["Address"]=rawT.Rows[i]["Address"].ToString();
				row["City"]=rawT.Rows[i]["City"].ToString();
				row["CarrierName"]=rawT.Rows[i]["CarrierName"].ToString();
				row["ElectID"]=rawT.Rows[i]["ElectID"].ToString();
				row["EmpName"]=rawT.Rows[i]["EmpName"].ToString();
				row["GroupName"]=rawT.Rows[i]["GroupName"].ToString();
				row["GroupNum"]=rawT.Rows[i]["GroupNum"].ToString();
				if(rawT.Rows[i]["NoSendElect"].ToString()=="0"){
					row["noSendElect"]="";
				}	else{
					row["noSendElect"]="X";
				}
				row["Phone"]=rawT.Rows[i]["Phone"].ToString();
				row["PlanNum"]=rawT.Rows[i]["onePlanNum"].ToString();
				row["plans"]=rawT.Rows[i]["plans"].ToString();
				row["State"]=rawT.Rows[i]["State"].ToString();
				row["TrojanID"]=rawT.Rows[i]["TrojanID"].ToString();
				row["Zip"]=rawT.Rows[i]["Zip"].ToString();
				rows.Add(row);
			}
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			return table;
		}

		///<summary>Used in FormFeesForIns</summary>
		public static DataTable GetListFeeCheck(string carrierName,string carrierNameNot,int feeSchedWithout,int feeSchedWith,
			FeeScheduleType feeSchedType)
		{
			string pFeeSched="FeeSched";
			if(feeSchedType==FeeScheduleType.Allowed){
				pFeeSched="AllowedFeeSched";
			}
			if(feeSchedType==FeeScheduleType.CoPay){
				pFeeSched="CopayFeeSched";
			}
			string command=
				"SELECT insplan.GroupName,insplan.GroupNum,COUNT(*) AS Plans,employer.EmpName,carrier.CarrierName,"
				+"insplan.EmployerNum,insplan.CarrierNum,feesched.Description AS FeeSchedName,insplan.PlanType,"
				+"insplan."+pFeeSched+" feeSched "
				+"FROM insplan "
				+"LEFT JOIN employer ON employer.EmployerNum = insplan.EmployerNum "
				+"LEFT JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum "
				+"LEFT JOIN feesched ON feesched.FeeSchedNum = insplan."+pFeeSched+" "
				+"WHERE carrier.CarrierName LIKE '%"+POut.PString(carrierName)+"%' ";
			if(carrierNameNot!=""){
				command+="AND carrier.CarrierName NOT LIKE '%"+POut.PString(carrierNameNot)+"%' ";
			}
			if(feeSchedWithout!=0){
				command+="AND insplan."+pFeeSched+" !="+POut.PInt(feeSchedWithout)+" ";
			}
			if(feeSchedWith!=0) {
				command+="AND insplan."+pFeeSched+" ="+POut.PInt(feeSchedWith)+" ";
			}
			command+="GROUP BY insplan.EmployerNum,insplan.GroupName,insplan.GroupNum,carrier.CarrierName,insplan.PlanType,"
				+"insplan."+pFeeSched+" "
				+"ORDER BY carrier.CarrierName,employer.EmpName,insplan.GroupNum";
			return Db.GetTable(command);
		}

		///<summary>Based on the four supplied parameters, it updates all similar plans.  Used in a specific tool: FormFeesForIns.</summary>
		public static int SetFeeSched(int employerNum,string carrierName,string groupNum,string groupName,int feeSchedNum,
			FeeScheduleType feeSchedType)
		{
			//FIXME:UPDATE-MULTIPLE-TABLES
			/*string command="UPDATE insplan,carrier SET insplan.FeeSched="+POut.PInt(feeSchedNum)
				+" WHERE carrier.CarrierNum = insplan.CarrierNum "//employer.EmployerNum = insplan.EmployerNum "
				+"AND insplan.EmployerNum='"+POut.PInt(employerNum)+"' "
				+"AND carrier.CarrierName='"+POut.PString(carrierName)+"' "
				+"AND insplan.GroupNum='"+POut.PString(groupNum)+"' "
				+"AND insplan.GroupName='"+POut.PString(groupName)+"'";
			 return Db.NonQ(command);
			 */
			//Code rewritten so that it is not only MySQL compatible, but Oracle compatible as well.
			string command="SELECT insplan.PlanNum FROM insplan,carrier "
				+"WHERE carrier.CarrierNum = insplan.CarrierNum "//employer.EmployerNum = insplan.EmployerNum "
				+"AND insplan.EmployerNum='"+POut.PInt(employerNum)+"' "
				+"AND carrier.CarrierName='"+POut.PString(carrierName)+"' "
				+"AND insplan.GroupNum='"+POut.PString(groupNum)+"' "
				+"AND insplan.GroupName='"+POut.PString(groupName)+"'";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return 0;
			}
			command="UPDATE insplan SET ";
			if(feeSchedType==FeeScheduleType.Normal){
				command+="insplan.FeeSched ="+POut.PInt(feeSchedNum)
					+" WHERE insplan.FeeSched !="+POut.PInt(feeSchedNum);
			}
			else if(feeSchedType==FeeScheduleType.Allowed){
				command+="insplan.AllowedFeeSched ="+POut.PInt(feeSchedNum)
					+" WHERE insplan.AllowedFeeSched !="+POut.PInt(feeSchedNum);
			}
			else if(feeSchedType==FeeScheduleType.CoPay){
				command+="insplan.CopayFeeSched ="+POut.PInt(feeSchedNum)
					+" WHERE insplan.CopayFeeSched !="+POut.PInt(feeSchedNum);
			}
			command+=" AND (";
			for(int i=0;i<table.Rows.Count;i++){
				command+="PlanNum="+table.Rows[i][0].ToString();
				if(i<table.Rows.Count-1){
					command+=" OR ";
				}
			}
			command+=")";
			return Db.NonQ(command);
		}

		///<summary>Returns number of rows affected.</summary>
		public static int ConvertToNewClaimform(int oldClaimFormNum, int newClaimFormNum){
			string command="UPDATE insplan SET ClaimFormNum="+POut.PInt(newClaimFormNum)
				+" WHERE ClaimFormNum="+POut.PInt(oldClaimFormNum);
			return Db.NonQ(command);
		}

		///<summary>Returns the number of fee schedules added.  It doesn't inform the user of how many plans were affected, but there will obviously be a certain number of plans for every new fee schedule.</summary>
		public static int GenerateAllowedFeeSchedules(){
			//get carrier names for all plans without an allowed fee schedule.
			string command="SELECT carrier.CarrierName "
				+"FROM insplan,carrier "
				+"WHERE carrier.CarrierNum=insplan.CarrierNum "
				+"AND insplan.AllowedFeeSched=0 "
				+"AND insplan.PlanType='' "
				+"GROUP BY carrier.CarrierName";
			DataTable table=Db.GetTable(command);
			//loop through all the carrier names
			string carrierName;
			FeeSched sched;
			int itemOrder=FeeSchedC.ListLong.Count;
			DataTable tableCarrierNums;
			int retVal=0;
			for(int i=0;i<table.Rows.Count;i++){
				carrierName=PIn.PString(table.Rows[i]["CarrierName"].ToString());
				if(carrierName=="" || carrierName==" "){
					continue;
				}
				//add a fee schedule if needed
				sched=FeeScheds.GetByExactName(carrierName,FeeScheduleType.Allowed);
				if(sched==null){
					sched=new FeeSched();
					sched.Description=carrierName;
					sched.FeeSchedType=FeeScheduleType.Allowed;
					sched.IsNew=true;
					sched.ItemOrder=itemOrder;
					FeeScheds.WriteObject(sched);
					itemOrder++;
				}
				//assign the fee sched to many plans
				//for compatibility with Oracle, get a list of all carrierNums that use the carriername
				command="SELECT CarrierNum FROM carrier WHERE CarrierName='"+POut.PString(carrierName)+"'";
				tableCarrierNums=Db.GetTable(command);
				if(tableCarrierNums.Rows.Count==0){
					continue;//I don't see how this could happen
				}
				command="UPDATE insplan "
					+"SET AllowedFeeSched="+POut.PInt(sched.FeeSchedNum)+" "
					+"WHERE AllowedFeeSched=0 "
					+"AND PlanType='' "
					+"AND (";
				for(int c=0;c<tableCarrierNums.Rows.Count;c++){
					if(c>0){
						command+=" OR ";
					}
					command+="CarrierNum="+tableCarrierNums.Rows[c]["CarrierNum"].ToString();
				}
				command+=")";
				retVal+=Db.NonQ(command);
			}
			return retVal;
		}

		public static int UnusedGetCount() {
			string command="SELECT COUNT(*) FROM insplan WHERE IsHidden=0 "
				+"AND NOT EXISTS (SELECT * FROM patplan WHERE patplan.PlanNum=insplan.PlanNum)";
			int count=PIn.PInt(Db.GetCount(command));
			return count;
		}

		public static void UnusedHideAll() {
			string command="UPDATE insplan SET IsHidden=1 "
				+"WHERE IsHidden=0 "
				+"AND NOT EXISTS (SELECT * FROM patplan WHERE patplan.PlanNum=insplan.PlanNum)";
			Db.NonQ(command);
		}

		//public static int GenerateOneAllowedFeeSchedule(){

		//}

		///<summary>Returns -1 if no copay feeschedule.  Can return -1 if copay amount is blank.</summary>
		public static double GetCopay(string myCode,InsPlan plan) {
			if(plan==null) {
				return -1;
			}
			if(plan.CopayFeeSched==0) {
				return -1;
			}
			double retVal=Fees.GetAmount(ProcedureCodes.GetCodeNum(myCode),plan.CopayFeeSched);
			if(retVal==-1) {//blank co-pay
				if(PrefC.GetBool("CoPay_FeeSchedule_BlankLikeZero")) {
					return -1;//will act like zero.  No patient co-pay.
				} else {
					//The amount from the regular fee schedule
					//In other words, the patient is responsible for procs that are not specified in a managed care fee schedule.
					return Fees.GetAmount(ProcedureCodes.GetCodeNum(myCode),plan.FeeSched);
				}
			}
			return retVal;
		}

		///<summary>Returns -1 if no allowed feeschedule or fee unknown for this procCode. Otherwise, returns the allowed fee including 0. Can handle a planNum of 0.  Tooth num is used for posterior composites.  It can be left blank in some situations.  Provider must be supplied in case plan has no assigned fee schedule.  Then it will use the fee schedule for the provider.</summary>
		public static double GetAllowed(string procCode,int planNum,List <InsPlan> PlanList,string toothNum,int provNum) {
			if(planNum==0) {
				return -1;
			}
			InsPlan plan=InsPlans.GetPlan(planNum,PlanList);
			if(plan==null) {
				return -1;
			}
			int codeNum=ProcedureCodes.GetCodeNum(procCode);
			int substCodeNum=codeNum;
			if(!plan.CodeSubstNone) {
				substCodeNum=ProcedureCodes.GetSubstituteCodeNum(procCode,toothNum);//for posterior composites
			}
			//PPO always returns the PPO fee for the code or substituted code.
			if(plan.PlanType=="p") {
				return Fees.GetAmount(substCodeNum,plan.FeeSched);
			}
			//or, if not PPO, and an allowed fee schedule exists, then we use that.
			if(plan.AllowedFeeSched!=0) {
				return Fees.GetAmount(substCodeNum,plan.AllowedFeeSched);//whether post composite or not
			}
			//must be an ordinary fee schedule, so if no substitution code, then no allowed override
			if(codeNum==substCodeNum) {
				return -1;
			}
			//must be posterior composite with an ordinary fee schedule
			//Although it won't happen very often, it's possible that there is no fee schedule assigned to the plan.
			if(plan.FeeSched==0) {
				return Fees.GetAmount(substCodeNum,Providers.GetProv(provNum).FeeSched);
			}
			return Fees.GetAmount(substCodeNum,plan.FeeSched);
		}

	}
}
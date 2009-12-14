﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenDentBusiness;

namespace OpenDental {
	public class ProcedureL {
		public static void SetCompleteInAppt(Appointment apt,List<InsPlan> PlanList,List<PatPlan> patPlans,long siteNum,int patientAge) {
			List<Procedure> procsInAppt=Procedures.GetProcsForSingle(apt.AptNum,false);
			Procedures.SetCompleteInAppt(apt,PlanList,patPlans,siteNum,patientAge,procsInAppt);
			List<string> procCodes=new List<string>();
			for(int i=0;i<procsInAppt.Count;i++){
				procCodes.Add(ProcedureCodes.GetStringProcCode(procsInAppt[i].CodeNum));
			}
			AutomationL.Trigger(AutomationTrigger.CompleteProcedure,procCodes,apt.PatNum);
		}


	}
}

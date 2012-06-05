//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class InstallmentPlanCrud {
		///<summary>Gets one InstallmentPlan object from the database using the primary key.  Returns null if not found.</summary>
		internal static InstallmentPlan SelectOne(long installmentPlanNum){
			string command="SELECT * FROM installmentplan "
				+"WHERE InstallmentPlanNum = "+POut.Long(installmentPlanNum);
			List<InstallmentPlan> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one InstallmentPlan object from the database using a query.</summary>
		internal static InstallmentPlan SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<InstallmentPlan> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of InstallmentPlan objects from the database using a query.</summary>
		internal static List<InstallmentPlan> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<InstallmentPlan> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<InstallmentPlan> TableToList(DataTable table){
			List<InstallmentPlan> retVal=new List<InstallmentPlan>();
			InstallmentPlan installmentPlan;
			for(int i=0;i<table.Rows.Count;i++) {
				installmentPlan=new InstallmentPlan();
				installmentPlan.InstallmentPlanNum= PIn.Long  (table.Rows[i]["InstallmentPlanNum"].ToString());
				installmentPlan.PatNum            = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				installmentPlan.DateAgreement     = PIn.Date  (table.Rows[i]["DateAgreement"].ToString());
				installmentPlan.DateFirstPayment  = PIn.Date  (table.Rows[i]["DateFirstPayment"].ToString());
				installmentPlan.MonthlyPayment    = PIn.Double(table.Rows[i]["MonthlyPayment"].ToString());
				installmentPlan.APR               = PIn.Float (table.Rows[i]["APR"].ToString());
				retVal.Add(installmentPlan);
			}
			return retVal;
		}

		///<summary>Inserts one InstallmentPlan into the database.  Returns the new priKey.</summary>
		internal static long Insert(InstallmentPlan installmentPlan){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				installmentPlan.InstallmentPlanNum=DbHelper.GetNextOracleKey("installmentplan","InstallmentPlanNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(installmentPlan,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							installmentPlan.InstallmentPlanNum++;
							loopcount++;
						}
						else{
							throw ex;
						}
					}
				}
				throw new ApplicationException("Insert failed.  Could not generate primary key.");
			}
			else {
				return Insert(installmentPlan,false);
			}
		}

		///<summary>Inserts one InstallmentPlan into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(InstallmentPlan installmentPlan,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				installmentPlan.InstallmentPlanNum=ReplicationServers.GetKey("installmentplan","InstallmentPlanNum");
			}
			string command="INSERT INTO installmentplan (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="InstallmentPlanNum,";
			}
			command+="PatNum,DateAgreement,DateFirstPayment,MonthlyPayment,APR) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(installmentPlan.InstallmentPlanNum)+",";
			}
			command+=
				     POut.Long  (installmentPlan.PatNum)+","
				+    POut.Date  (installmentPlan.DateAgreement)+","
				+    POut.Date  (installmentPlan.DateFirstPayment)+","
				+"'"+POut.Double(installmentPlan.MonthlyPayment)+"',"
				+    POut.Float (installmentPlan.APR)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				installmentPlan.InstallmentPlanNum=Db.NonQ(command,true);
			}
			return installmentPlan.InstallmentPlanNum;
		}

		///<summary>Updates one InstallmentPlan in the database.</summary>
		internal static void Update(InstallmentPlan installmentPlan){
			string command="UPDATE installmentplan SET "
				+"PatNum            =  "+POut.Long  (installmentPlan.PatNum)+", "
				+"DateAgreement     =  "+POut.Date  (installmentPlan.DateAgreement)+", "
				+"DateFirstPayment  =  "+POut.Date  (installmentPlan.DateFirstPayment)+", "
				+"MonthlyPayment    = '"+POut.Double(installmentPlan.MonthlyPayment)+"', "
				+"APR               =  "+POut.Float (installmentPlan.APR)+" "
				+"WHERE InstallmentPlanNum = "+POut.Long(installmentPlan.InstallmentPlanNum);
			Db.NonQ(command);
		}

		///<summary>Updates one InstallmentPlan in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(InstallmentPlan installmentPlan,InstallmentPlan oldInstallmentPlan){
			string command="";
			if(installmentPlan.PatNum != oldInstallmentPlan.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(installmentPlan.PatNum)+"";
			}
			if(installmentPlan.DateAgreement != oldInstallmentPlan.DateAgreement) {
				if(command!=""){ command+=",";}
				command+="DateAgreement = "+POut.Date(installmentPlan.DateAgreement)+"";
			}
			if(installmentPlan.DateFirstPayment != oldInstallmentPlan.DateFirstPayment) {
				if(command!=""){ command+=",";}
				command+="DateFirstPayment = "+POut.Date(installmentPlan.DateFirstPayment)+"";
			}
			if(installmentPlan.MonthlyPayment != oldInstallmentPlan.MonthlyPayment) {
				if(command!=""){ command+=",";}
				command+="MonthlyPayment = '"+POut.Double(installmentPlan.MonthlyPayment)+"'";
			}
			if(installmentPlan.APR != oldInstallmentPlan.APR) {
				if(command!=""){ command+=",";}
				command+="APR = "+POut.Float(installmentPlan.APR)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE installmentplan SET "+command
				+" WHERE InstallmentPlanNum = "+POut.Long(installmentPlan.InstallmentPlanNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one InstallmentPlan from the database.</summary>
		internal static void Delete(long installmentPlanNum){
			string command="DELETE FROM installmentplan "
				+"WHERE InstallmentPlanNum = "+POut.Long(installmentPlanNum);
			Db.NonQ(command);
		}

	}
}
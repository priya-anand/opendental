using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Deposits {

		///<summary>Gets all Deposits, ordered by date.  </summary>
		public static Deposit[] Refresh() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Deposit[]>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM deposit "
				+"ORDER BY DateDeposit";
			return RefreshAndFill(command);
		}

		///<summary>Gets only Deposits which are not attached to transactions.</summary>
		public static Deposit[] GetUnattached() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Deposit[]>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM deposit "
				+"WHERE NOT EXISTS(SELECT * FROM transaction WHERE deposit.DepositNum=transaction.DepositNum) "
				+"ORDER BY DateDeposit";
			return RefreshAndFill(command);
		}

		///<summary>Gets a single deposit directly from the database.</summary>
		public static Deposit GetOne(int depositNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Deposit>(MethodBase.GetCurrentMethod(),depositNum);
			}
			string command="SELECT * FROM deposit "
				+"WHERE DepositNum="+POut.PInt(depositNum);
			return RefreshAndFill(command)[0];
		}

		private static Deposit[] RefreshAndFill(string command) {
			DataTable table=Db.GetTable(command);
			Deposit[] List=new Deposit[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				List[i]=new Deposit();
				List[i].DepositNum     = PIn.PInt(table.Rows[i][0].ToString());
				List[i].DateDeposit    = PIn.PDate(table.Rows[i][1].ToString());
				List[i].BankAccountInfo= PIn.PString(table.Rows[i][2].ToString());
				List[i].Amount         = PIn.PDouble(table.Rows[i][3].ToString());
			}
			return List;
		}

		///<summary></summary>
		public static void Update(Deposit dep){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dep);
				return;
			}
			string command= "UPDATE deposit SET "
				+"DateDeposit = "     +POut.PDate  (dep.DateDeposit)
				+",BankAccountInfo = '"+POut.PString(dep.BankAccountInfo)+"'"
				+",Amount = '"         +POut.PDouble(dep.Amount)+"'"
				+" WHERE DepositNum ='"+POut.PInt   (dep.DepositNum)+"'";
			//MessageBox.Show(string command);
 			Db.NonQ(command);
		}

		///<summary></summary>
		public static void Insert(Deposit dep){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dep);
				return;
			}
			if(PrefC.RandomKeys){
				dep.DepositNum=MiscData.GetKey("deposit","DepositNum");
			}
			string command= "INSERT INTO deposit (";
			if(PrefC.RandomKeys){
				command+="DepositNum,";
			}
			command+="DateDeposit,BankAccountInfo,Amount) VALUES(";
			if(PrefC.RandomKeys){
				command+="'"+POut.PInt(dep.DepositNum)+"', ";
			}
			command+=
				 POut.PDate  (dep.DateDeposit)+", "
				+"'"+POut.PString(dep.BankAccountInfo)+"', "
				+"'"+POut.PDouble(dep.Amount)+"')";
 			if(PrefC.RandomKeys){
				Db.NonQ(command);
			}
			else{
 				dep.DepositNum=Db.NonQ(command,true);
			}
		}

		///<summary>Also handles detaching all payments and claimpayments.  Throws exception if deposit is attached as a source document to a transaction.  The program should have detached the deposit from the transaction ahead of time, so I would never expect the program to throw this exception unless there was a bug.</summary>
		public static void Delete(Deposit dep){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dep);
				return;
			}
			//check dependencies
			string command="";
			if(dep.DepositNum !=0){
				command="SELECT COUNT(*) FROM transaction WHERE DepositNum ="+POut.PInt(dep.DepositNum);
				if(PIn.PInt(Db.GetCount(command))>0) {
					throw new ApplicationException(Lan.g("Deposits","Cannot delete deposit because it is attached to a transaction."));
				}
			}
			/*/check claimpayment 
			command="SELECT COUNT(*) FROM claimpayment WHERE DepositNum ="+POut.PInt(DepositNum);
			if(PIn.PInt(Db.GetCount(command))>0){
				throw new InvalidProgramException(Lan.g("Deposits","Cannot delete deposit because it has payments attached"));
			}*/
			//ready to delete
			command="UPDATE payment SET DepositNum=0 WHERE DepositNum="+POut.PInt(dep.DepositNum);
			Db.NonQ(command);
			command="UPDATE claimpayment SET DepositNum=0 WHERE DepositNum="+POut.PInt(dep.DepositNum);
			Db.NonQ(command);
			command= "DELETE FROM deposit WHERE DepositNum="+POut.PInt(dep.DepositNum);
 			Db.NonQ(command);
		}



	



	
	}

	

	


}





















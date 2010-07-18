//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class PatFieldDefCrud {
		///<summary>Gets one PatFieldDef object from the database using the primary key.  Returns null if not found.</summary>
		internal static PatFieldDef SelectOne(long patFieldDefNum){
			string command="SELECT * FROM patfielddef "
				+"WHERE PatFieldDefNum = "+POut.Long(patFieldDefNum)+" LIMIT 1";
			List<PatFieldDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PatFieldDef object from the database using a query.</summary>
		internal static PatFieldDef SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PatFieldDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PatFieldDef objects from the database using a query.</summary>
		internal static List<PatFieldDef> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PatFieldDef> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<PatFieldDef> TableToList(DataTable table){
			List<PatFieldDef> retVal=new List<PatFieldDef>();
			PatFieldDef patFieldDef;
			for(int i=0;i<table.Rows.Count;i++) {
				patFieldDef=new PatFieldDef();
				patFieldDef.PatFieldDefNum= PIn.Long  (table.Rows[i]["PatFieldDefNum"].ToString());
				patFieldDef.FieldName     = PIn.String(table.Rows[i]["FieldName"].ToString());
				retVal.Add(patFieldDef);
			}
			return retVal;
		}

		///<summary>Inserts one PatFieldDef into the database.  Returns the new priKey.</summary>
		internal static long Insert(PatFieldDef patFieldDef){
			return Insert(patFieldDef,false);
		}

		///<summary>Inserts one PatFieldDef into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(PatFieldDef patFieldDef,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				patFieldDef.PatFieldDefNum=ReplicationServers.GetKey("patfielddef","PatFieldDefNum");
			}
			string command="INSERT INTO patfielddef (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PatFieldDefNum,";
			}
			command+="FieldName) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(patFieldDef.PatFieldDefNum)+",";
			}
			command+=
				 "'"+POut.String(patFieldDef.FieldName)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				patFieldDef.PatFieldDefNum=Db.NonQ(command,true);
			}
			return patFieldDef.PatFieldDefNum;
		}

		///<summary>Updates one PatFieldDef in the database.</summary>
		internal static void Update(PatFieldDef patFieldDef){
			string command="UPDATE patfielddef SET "
				+"FieldName     = '"+POut.String(patFieldDef.FieldName)+"' "
				+"WHERE PatFieldDefNum = "+POut.Long(patFieldDef.PatFieldDefNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one PatFieldDef in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(PatFieldDef patFieldDef,PatFieldDef oldPatFieldDef){
			string command="";
			if(patFieldDef.FieldName != oldPatFieldDef.FieldName) {
				if(command!=""){ command+=",";}
				command+="FieldName = '"+POut.String(patFieldDef.FieldName)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE patfielddef SET "+command
				+" WHERE PatFieldDefNum = "+POut.Long(patFieldDef.PatFieldDefNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one PatFieldDef from the database.</summary>
		internal static void Delete(long patFieldDefNum){
			string command="DELETE FROM patfielddef "
				+"WHERE PatFieldDefNum = "+POut.Long(patFieldDefNum)+" LIMIT 1";
			Db.NonQ(command);
		}

	}
}
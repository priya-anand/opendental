using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	///<summary>Replaces old "General" class.  Used to send queries.  The methods will soon be internal since it is no longer acceptable for the UI to be sending queries.</summary>
	public class Db {
		///<summary></summary>
		internal static DataTable GetTable(string command) {
			DataTable retVal;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("No longer allowed to send sql directly.  For user sql, use GetTableLow.  Othewise, rewrite the calling class to not use this query:\r\n"+command);
			}
			else{
				retVal=DataCore.GetTable(command);
			}
			retVal.TableName="";//this is needed for FormQuery dataGrid
			return retVal;
		}

		///<summary>This is used for queries written by the user.  If using direct connection, it gets a table in the ordinary way.  If ServerWeb, it uses the user with lower privileges to prevent injection attack.</summary>
		internal static DataTable GetTableLow(string command) {
			DataTable retVal;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Rewrite the calling class to pass this query off to the server:\r\n"+command);
			}
			else if(RemotingClient.RemotingRole==RemotingRole.ClientDirect) {
				retVal=DataCore.GetTable(command);
			}
			else {//ServerWeb
				retVal=DataCore.GetTableLow(command);
			}
			retVal.TableName="";//this is needed for FormQuery dataGrid
			return retVal;
		}

		///<summary>This is for multiple queries all concatenated together with ;</summary>
		internal static DataSet GetDataSet(string commands) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("No longer allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+commands);
			}
			else {
				return DataCore.GetDataSet(commands);
			}
		}

		///<summary>This query is run with full privileges.  This is for commands generated by the main program, and the user will not have access for injection attacks.  Result is usually number of rows changed, or can be insert id if requested.</summary>
		internal static int NonQ(string command,bool getInsertID) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("No longer allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			else {
				return DataCore.NonQ(command,getInsertID);
			}
		}

		internal static int NonQ(string command) {
			return NonQ(command,false);
		}

		///<summary>We need to get away from this due to poor support from databases.  For now, each command will be sent entirely separately.  This never returns number of rows affected.</summary>
		internal static int NonQ(string[] commands) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("No longer allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+commands[0]);
			}
			for(int i=0;i<commands.Length;i++) {
				DataCore.NonQ(commands[i],false);
			}
			return 0;
		}

		///<summary>Use this for count(*) queries.  They are always guaranteed to return one and only one value.  Not any faster, just handier.  Can also be used when retrieving prefs manually, since they will also return exactly one value.</summary>
		internal static string GetCount(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("No longer allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			else {
				return DataCore.GetTable(command).Rows[0][0].ToString();
			}
		}

		///<summary>Use this only for queries that return one value.</summary>
		internal static string GetScalar(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("No longer allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			else {
				return DataCore.GetScalar(command);
			}
		}

		#region old
		public static DataTable GetTableOld(string command) {
			throw new ApplicationException("No queries allowed in the UI layer.");
		}

		public static int NonQOld(string[] commands) {
			throw new ApplicationException("No queries allowed in the UI layer.");
		}

		public static int NonQOld(string command) {
			throw new ApplicationException("No queries allowed in the UI layer.");
		}
		#endregion old


	}
}

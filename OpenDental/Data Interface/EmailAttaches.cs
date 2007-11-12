using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class EmailAttaches{

		public static void Insert(EmailAttach attach) {
			if(PrefB.RandomKeys) {
				attach.EmailAttachNum=MiscData.GetKey("emailattach","EmailAttachNum");
			}
			string command= "INSERT INTO emailattach (";
			if(PrefB.RandomKeys) {
				command+="EmailAttachNum,";
			}
			command+="EmailMessageNum, DisplayedFileName, ActualFileName) VALUES(";
			if(PrefB.RandomKeys) {
				command+="'"+POut.PInt(attach.EmailAttachNum)+"', ";
			}
			command+=
				 "'"+POut.PInt(attach.EmailMessageNum)+"', "
				+"'"+POut.PString(attach.DisplayedFileName)+"', "
				+"'"+POut.PString(attach.ActualFileName)+"')";
			if(PrefB.RandomKeys) {
				General.NonQ(command);
			}
			else{
				attach.EmailAttachNum=General.NonQ(command,true);
			}
		}


	}

	


}










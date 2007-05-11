using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class Schedules {
		///<summary>Used in the schedule setup window</summary>
		public static Schedule[] RefreshMonth(DateTime CurDate,ScheduleType schedType,int provNum) {
			string command=
				//"SELECT * FROM schedule WHERE SchedDate > '"+POut.PDate(startDate.AddDays(-1))+"' "
				//+"AND SchedDate < '"+POut.PDate(stopDate.AddDays(1))+"' "
				/*"SELECT * FROM schedule WHERE MONTH(SchedDate)='"+CurDate.Month.ToString()
				+"' AND YEAR(SchedDate)='"+CurDate.Year.ToString()+"' "
				+"AND SchedType="+POut.PInt((int)schedType)
				+" AND ProvNum="+POut.PInt(provNum)
				+" ORDER BY starttime";*/
				"SELECT * FROM schedule WHERE ";
			if(FormChooseDatabase.DBtype==DatabaseType.Oracle){
				command+="TO_CHAR(SchedDate,'MM')='"+CurDate.Month.ToString()+"' AND TO_CHAR(SchedDate,'YYYY')";
			}else{//Assume MySQL
				command+="MONTH(SchedDate)='"+CurDate.Month.ToString()+"' AND YEAR(SchedDate)";
			}
			command+="='"+CurDate.Year.ToString()+"'"
				+" AND SchedType="+POut.PInt((int)schedType)
				+" AND ProvNum="+POut.PInt(provNum)
				+" ORDER BY starttime";
			return RefreshAndFill(command);
		}

		///<summary>Called every time the day is refreshed or changed in Appointments module.  Gets the data directly from the database.</summary>
		public static Schedule[] RefreshDay(DateTime thisDay) {
			string command=
				"SELECT * FROM schedule WHERE SchedDate="+POut.PDate(thisDay)
				+" ORDER BY starttime";
			return RefreshAndFill(command);
		}

		///<summary>Called every time the period is refreshed or changed in Appointments module.  Gets the data directly from the database.</summary>
		public static Schedule[] RefreshPeriod(DateTime startDate,DateTime endDate) {
			string command="SELECT * FROM schedule WHERE SchedDate BETWEEN '"
				+POut.PDate(startDate,false)+"' AND '"+POut.PDate(endDate.AddDays(1),false)+"'"
				+" ORDER BY starttime";
			return RefreshAndFill(command);
		}

		///<summary>Used in the check database integrity tool.</summary>
		public static Schedule[] RefreshAll() {
			string command="SELECT * FROM schedule";
			return RefreshAndFill(command);
		}

		private static Schedule[] RefreshAndFill(string command) {
			DataTable table=General.GetTableEx(command);
			Schedule[] List=new Schedule[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				List[i]=new Schedule();
				List[i].ScheduleNum    = PIn.PInt(table.Rows[i][0].ToString());
				List[i].SchedDate      = PIn.PDate(table.Rows[i][1].ToString());
				List[i].StartTime      = PIn.PDateT(table.Rows[i][2].ToString());
				List[i].StopTime       = PIn.PDateT(table.Rows[i][3].ToString());
				List[i].SchedType      = (ScheduleType)PIn.PInt(table.Rows[i][4].ToString());
				List[i].ProvNum        = PIn.PInt(table.Rows[i][5].ToString());
				List[i].BlockoutType   = PIn.PInt(table.Rows[i][6].ToString());
				List[i].Note           = PIn.PString(table.Rows[i][7].ToString());
				List[i].Status         = (SchedStatus)PIn.PInt(table.Rows[i][8].ToString());
				List[i].Op             = PIn.PInt(table.Rows[i][9].ToString());
			}
			return List;
		}

		///<summary></summary>
		private static void Update(Schedule sched){
			string command= "UPDATE schedule SET " 
				+ "scheddate = "    +POut.PDate  (sched.SchedDate)
				+ ",starttime = "   +POut.PDateT (sched.StartTime)
				+ ",stoptime = "    +POut.PDateT (sched.StopTime)
				+ ",SchedType = '"   +POut.PInt   ((int)sched.SchedType)+"'"
				+ ",ProvNum = '"     +POut.PInt   (sched.ProvNum)+"'"
				+ ",BlockoutType = '"+POut.PInt   (sched.BlockoutType)+"'"
				+ ",note = '"        +POut.PString(sched.Note)+"'"
				+ ",status = '"      +POut.PInt   ((int)sched.Status)+"'"
				+ ",Op = '"          +POut.PInt   (sched.Op)+"'"
				+" WHERE ScheduleNum = '" +POut.PInt (sched.ScheduleNum)+"'";
 			General.NonQ(command);
		}

		///<summary>This should not be used from outside this class because it doesn't validate.  Use InsertOrUpdate instead.</summary>
		public static void Insert(Schedule sched){
			if(PrefB.RandomKeys){
				sched.ScheduleNum=MiscData.GetKey("schedule","ScheduleNum");
			}
			string command= "INSERT INTO schedule (";
			if(PrefB.RandomKeys){
				command+="ScheduleNum,";
			}
			command+="scheddate,starttime,stoptime,"
				+"SchedType,ProvNum,BlockoutType,Note,Status,Op) VALUES(";
			if(PrefB.RandomKeys){
				command+="'"+POut.PInt(sched.ScheduleNum)+"', ";
			}
			command+=
				 POut.PDate  (sched.SchedDate)+", "
				+POut.PDateT (sched.StartTime)+", "
				+POut.PDateT (sched.StopTime)+", "
				+"'"+POut.PInt   ((int)sched.SchedType)+"', "
				+"'"+POut.PInt   (sched.ProvNum)+"', "
				+"'"+POut.PInt   (sched.BlockoutType)+"', "
				+"'"+POut.PString(sched.Note)+"', "
				+"'"+POut.PInt   ((int)sched.Status)+"', "
				+"'"+POut.PInt   (sched.Op)+"')";
			if(PrefB.RandomKeys) {
				General.NonQ(command);
			}
			else {
				sched.ScheduleNum=General.NonQ(command,true);
			}
		}

		///<summary></summary>
		public static void InsertOrUpdate(Schedule sched, bool isNew){
			if(sched.StartTime.TimeOfDay > sched.StopTime.TimeOfDay){
				throw new Exception(Lan.g("Schedule","Stop time must be later than start time."));
			}
			if(sched.StartTime.TimeOfDay+TimeSpan.FromMinutes(5) > sched.StopTime.TimeOfDay
				&& sched.Status==SchedStatus.Open)
			{
				throw new Exception(Lan.g("Schedule","Stop time cannot be the same as the start time."));
			}
			if(Overlaps(sched)){
				throw new Exception(Lan.g("Schedule","Cannot overlap another time block."));
			}
			if(isNew){
				Insert(sched);
			}
			else{
				Update(sched);
			}
		}

		///<summary></summary>
		private static bool Overlaps(Schedule sched){
			Schedule[] SchedListDay=Schedules.RefreshDay(sched.SchedDate);
			Schedule[] ListForType=Schedules.GetForType(SchedListDay,sched.SchedType,sched.ProvNum);
			for(int i=0;i<ListForType.Length;i++){
				if(ListForType[i].SchedType==ScheduleType.Blockout){
					//skip if blockout, and ops don't interfere
					if(sched.Op!=0 && ListForType[i].Op!=0){//neither op can be zero, or they will interfere
						if(sched.Op != ListForType[i].Op){
							continue;
						}
					}
				}
				if(sched.ScheduleNum!=ListForType[i].ScheduleNum
					&& sched.StartTime.TimeOfDay >= ListForType[i].StartTime.TimeOfDay
					&& sched.StartTime.TimeOfDay < ListForType[i].StopTime.TimeOfDay)
				{
					return true;
				}
				if(sched.ScheduleNum!=ListForType[i].ScheduleNum
					&& sched.StopTime.TimeOfDay > ListForType[i].StartTime.TimeOfDay
					&& sched.StopTime.TimeOfDay <= ListForType[i].StopTime.TimeOfDay)
				{
					return true;
				}
				if(sched.ScheduleNum!=ListForType[i].ScheduleNum
					&& sched.StartTime.TimeOfDay <= ListForType[i].StartTime.TimeOfDay
					&& sched.StopTime.TimeOfDay >= ListForType[i].StopTime.TimeOfDay)
				{
					return true;
				}
			}
			return false;
		}

		///<summary>Also automatically handles situation where the last blockout for the day gets deleted.  In that case, it adds a "closed" blockout to signify an override of default blockouts.</summary>
		public static void 
			Delete(Schedule sched){
			string command= "DELETE from schedule WHERE schedulenum = '"+POut.PInt(sched.ScheduleNum)+"'";
 			General.NonQ(command);
			//if this was the last blockout for a day, then create a blockout for 'closed'
			if(sched.SchedType==ScheduleType.Blockout){
				Schedules.CheckIfDeletedLastBlockout(sched.SchedDate);
			}
		}



	
		///<summary>Supply a list of all Schedule for one day. Then, this filters out for one type.</summary>
		public static Schedule[] GetForType(Schedule[] ListDay,ScheduleType schedType,int provNum){
			ArrayList AL=new ArrayList();
			for(int i=0;i<ListDay.Length;i++){
				if(ListDay[i].SchedType==schedType && ListDay[i].ProvNum==provNum){
					AL.Add(ListDay[i]);
				}
			}
			Schedule[] retVal=new Schedule[AL.Count];
			AL.CopyTo(retVal);
			return retVal;
		}

		///<summary>If a particular day already has some non-default schedule items, then this does nothing and returns false.  But if the day is all default, then it converts each default entry into an actual schedule entry and returns true.  The user would not notice this change, but now they can edit or add.</summary>
		public static bool ConvertFromDefault(DateTime forDate,ScheduleType schedType,int provNum){
			Schedule[] List=RefreshDay(forDate);
			Schedule[] ListForType=GetForType(List,schedType,provNum);
			if(ListForType.Length>0){
				return false;//do nothing, since it has already been converted from default.
				//it is also possible there will be no default entries to convert, but that's ok.
			}
			SchedDefault[] ListDefaults=SchedDefaults.GetForType(schedType,provNum);
			for(int i=0;i<ListDefaults.Length;i++){
				if(ListDefaults[i].DayOfWeek!=(int)forDate.DayOfWeek){
					continue;//if day of week doesn't match, then skip
				}
				Schedule SchedCur=new Schedule();
				SchedCur.Status=SchedStatus.Open;
				SchedCur.SchedDate=forDate;
				SchedCur.StartTime=ListDefaults[i].StartTime;
				SchedCur.StopTime=ListDefaults[i].StopTime; 
				SchedCur.SchedType=schedType;
				SchedCur.ProvNum=provNum;
				SchedCur.Op=ListDefaults[i].Op;
				SchedCur.BlockoutType=ListDefaults[i].BlockoutType;
				InsertOrUpdate(SchedCur,true);     
			}
			return true;
		}

		///<summary></summary>
		public static void SetAllDefault(DateTime forDate,ScheduleType schedType,int provNum){
			string command="DELETE from schedule WHERE "
				+"SchedDate="    +POut.PDate (forDate)+" "
				+"AND SchedType='"+POut.PInt((int)schedType)+"' "
				+"AND ProvNum='"  +POut.PInt(provNum)+"'";
 			General.NonQ(command);
		}

		///<summary>Clears all blockouts for day.  But then defaults would show.  So adds a "closed" blockout.</summary>
		public static void ClearBlockoutsForDay(DateTime date){
			SetAllDefault(date,ScheduleType.Blockout,0);
			CheckIfDeletedLastBlockout(date);
		}

		///<summary></summary>
		public static void CheckIfDeletedLastBlockout(DateTime schedDate){
			string command="SELECT COUNT(*) FROM schedule WHERE SchedType='"+POut.PInt((int)ScheduleType.Blockout)+"' "
					+"AND SchedDate="+POut.PDate(schedDate);
			DataTable table=General.GetTable(command);
			if(table.Rows[0][0].ToString()=="0") {
				Schedule sched=new Schedule();
				sched.SchedDate=schedDate;
				sched.SchedType=ScheduleType.Blockout;
				sched.Status=SchedStatus.Closed;
				Insert(sched);
			}
		}

		public static bool DateIsHoliday(DateTime date){
			string command="SELECT COUNT(*) FROM schedule WHERE Status=2 "//holiday
				+"AND SchedDate= "+POut.PDate(date);
			string result=General.GetCount(command);
			if(result=="0"){
				return false;
			}
			return true;
		}

		///<Summary>Returns a 7 column data table in a calendar layout so all you have to do is draw it on the screen.</Summary>
		public static DataTable GetPeriod(DateTime dateStart,DateTime dateEnd,List<int> provNums){
			DataTable table=new DataTable();
			DataRow row;
			table.Columns.Add("sun");
			table.Columns.Add("mon");
			table.Columns.Add("tues");
			table.Columns.Add("wed");
			table.Columns.Add("thurs");
			table.Columns.Add("fri");
			table.Columns.Add("sat");
			string command="SELECT Abbr,SchedDate,StartTime,StopTime "
				+"FROM schedule,provider "
				+"WHERE schedule.ProvNum=provider.ProvNum "
				+"AND SchedDate >= "+POut.PDate(dateStart)+" "
				+"AND SchedDate <= "+POut.PDate(dateEnd)+" "
				+"AND SchedType=1 ";
			for(int i=0;i<provNums.Count;i++){
				if(i==0){
					command+="AND (";
				}
				else{
					command+=" OR ";
				}
				command+="schedule.ProvNum="+POut.PInt(provNums[i]);
				if(i==provNums.Count-1){
					command+=") ";
				}
			}
			command+="ORDER BY SchedDate,provider.ItemOrder,StartTime";
			DataTable raw=General.GetTable(command);
			DateTime dateSched;
			DateTime startTime;
			DateTime stopTime;
			int rowsInGrid=GetRowCal(dateStart,dateEnd)+1;//because 0-based
			for(int i=0;i<rowsInGrid;i++){
				row=table.NewRow();
				table.Rows.Add(row);
			}
			dateSched=dateStart;
			while(dateSched<=dateEnd){
				table.Rows[GetRowCal(dateStart,dateSched)][(int)dateSched.DayOfWeek]=
					dateSched.ToString("MMM d");
				dateSched=dateSched.AddDays(1);
			}
			int rowI;
			for(int i=0;i<raw.Rows.Count;i++){
				dateSched=PIn.PDate(raw.Rows[i]["SchedDate"].ToString());
				startTime=PIn.PDateT(raw.Rows[i]["StartTime"].ToString());
				stopTime=PIn.PDateT(raw.Rows[i]["StopTime"].ToString());
				rowI=GetRowCal(dateStart,dateSched);
				//table.Rows[rowI][(int)dateSched.DayOfWeek]+=;
				if(i==0){
					table.Rows[rowI][(int)dateSched.DayOfWeek]+="\r\n"+raw.Rows[i]["Abbr"].ToString()+" ";
				}
				else if(raw.Rows[i-1]["Abbr"].ToString()==raw.Rows[i]["Abbr"].ToString()
					&& raw.Rows[i-1]["SchedDate"].ToString()==raw.Rows[i]["SchedDate"].ToString())
				{
					table.Rows[rowI][(int)dateSched.DayOfWeek]+=", ";
				}
				else{
					table.Rows[rowI][(int)dateSched.DayOfWeek]+="\r\n"+raw.Rows[i]["Abbr"].ToString()+" ";
				}
				table.Rows[rowI][(int)dateSched.DayOfWeek]+=
					startTime.ToString("h:mm")+"-"+stopTime.ToString("h:mm");
			}
			return table;
		}

		///<summary>Returns the 0-based row where endDate will fall in a calendar grid.  It is not necessary to have a function to retrieve the column, because that is simply (int)myDate.DayOfWeek</summary>
		public static int GetRowCal(DateTime startDate,DateTime endDate){
			TimeSpan span=endDate-startDate;
			int dayInterval=span.Days;
			int daysFirstWeek=7-(int)startDate.DayOfWeek;//eg Monday=7-1=6.  or Sat=7-6=1.
			dayInterval=dayInterval-daysFirstWeek;
			if(dayInterval<0){
				return 0;
			}
			return (int)Math.Ceiling((dayInterval+1)/7d);
		}

		///<Summary>When click on a calendar grid, this is used to calculate the date clicked on.  StartDate is the first date in the Calendar, which does not have to be Sun.</Summary>
		public static DateTime GetDateCal(DateTime startDate,int row,int col){
			DateTime dateFirstRow;//the first date of row 0. Typically a few days before startDate. Always a Sun.
			dateFirstRow=startDate.AddDays(-(int)startDate.DayOfWeek);//example: (Tues,May 9).AddDays(-2)=Sun,May 7.
			return dateFirstRow.AddDays(row*7+col);
		}
		
	}

	

	

}














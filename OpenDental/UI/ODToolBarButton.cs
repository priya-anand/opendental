using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental.UI{ 

	///<summary>Almost the same as the included ToolBarButton, but with a few extra properties.</summary>
	public class ODToolBarButton : System.ComponentModel.Component{
		///<summary>Pushed(toggle) and enabled are handled separately</summary>
		private ToolBarButtonState state;
		private ODToolBarButtonStyle style;
		private int imageIndex;
		private Rectangle bounds;
		private string text;
		private string toolTipText;
		private bool enabled=true;
		private Menu dropDownMenu;
		private Object tag;
		private bool pushed;
	  
		///<summary>Creates a new ODToolBarButton.</summary>
		public ODToolBarButton(){
			style=ODToolBarButtonStyle.PushButton;
			state=ToolBarButtonState.Normal;
			imageIndex=-1;
			text="";
			toolTipText="";
			tag="";
		}

		///<summary>Creates a new ODToolBarButton with the given text.</summary>
		public ODToolBarButton(string buttonText,int buttonImageIndex,string buttonToolTip,Object buttonTag){
			style=ODToolBarButtonStyle.PushButton;
			state=ToolBarButtonState.Normal;
			imageIndex=buttonImageIndex;
			text=buttonText;
			toolTipText=buttonToolTip;
			tag=buttonTag;
		}

		///<summary>Creates a new separator style ODToolBarButton.</summary>
		public ODToolBarButton(ODToolBarButtonStyle buttonStyle){
			style=buttonStyle;
			state=ToolBarButtonState.Normal;
			imageIndex=-1;
			text="";
			toolTipText="";
			tag="";
		}


		///<summary>The bounds of this button.</summary>
		public Rectangle Bounds{
			get{
				return bounds;
			}
			set{
				bounds=value;
			}
		}

		///<summary></summary>
		public ODToolBarButtonStyle Style{
			get{
				return style;
			}
			set{
				style=value;
			}
		}

		///<summary></summary>
		public ToolBarButtonState State{
			get{
				return state;
			}
			set{
				state=value;
			}
		}

		///<summary></summary>
		public string Text{
			get{
				return text;
			}
			set{
				text=value;
			}
		}

		///<summary></summary>
		public string ToolTipText{
			get{
				return toolTipText;
			}
			set{
				toolTipText=value;
			}
		}


		///<summary></summary>
		public int ImageIndex{
			get{
				return imageIndex;
			}
			set{
				imageIndex=value;
			}
		}

		///<summary></summary>
		public bool Enabled{
			get{
				return enabled;
			}
			set{
				enabled=value;
			}
		}
	  
		///<summary></summary>
		public Menu DropDownMenu{
			get{
				return dropDownMenu;
			}
			set{
				dropDownMenu=value;
			}
		}

		///<summary>Holds extra information about the button, so we can tell which button was clicked.</summary>
		public Object Tag{
			get{
				return tag;
			}
			set{
				tag=value;
			}
		}

		///<summary>Only used if style is ToggleButton.</summary>
		public bool Pushed{
			get{
				return pushed;
			}
			set{
				pushed=value;
			}
		}
	        
	        

	}

	///<summary>There are also pushed and enabled to worry about separately.</summary>
	public enum ToolBarButtonState{
		///<summary>0.</summary>
		Normal,
		///<summary>Mouse is hovering over the button and the mouse button is not pressed.</summary>
		Hover,
		///<summary>Mouse was pressed over this button and is still down, even if it has moved off this button or off the toolbar.</summary>
		Pressed,
		///<summary>In a dropdown button, only the dropdown portion is pressed. For hover, the entire button acts as one, but for pressing, the dropdown can be pressed separately.</summary>
		DropPressed
	}

	///<summary>Just like Forms.ToolBarButtonStyle, except includes some extras.</summary>
	public enum ODToolBarButtonStyle{
		///<summary>A button with a dropdown list on the right.</summary>
		DropDownButton,
		///<summary>A standard button</summary>
		PushButton,
		///<summary></summary>
		Separator,
		///<summary>Toggles between pushed and not pushed when clicked on.</summary>
		ToggleButton,
		///<summary>Not clickable. Just text where a button would normally be. Can also include an image.</summary>
		Label
	}

}







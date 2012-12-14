/*=============================================================================================================
Open Dental's Web Application is a dental practice management web app.
Copyright (C) 2012 Jordan Sparks, DMD.  http://www.opendental.com

This program is free software; you can redistribute it and/or modify it under the terms of the
GNU Db Public License as published by the Free Software Foundation; either version 2 of the License,
or (at your option) any later version.

This program is distributed in the hope that it will be useful, but without any warranty. See the GNU Db Public License
for more details, available at http://www.opensource.org/licenses/gpl-license.php

Any changes to this program must follow the guidelines of the GPL license if a modified version is to be
redistributed.

Web app programmer: Jason Salmon
===============================================================================================================*/

package com.opendental.odweb.client.mainmodules;

import java.util.ArrayList;

import com.google.gwt.core.client.GWT;
import com.google.gwt.event.shared.HandlerRegistration;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.user.client.ui.ResizeComposite;
import com.google.gwt.user.client.ui.SimpleLayoutPanel;
import com.google.gwt.user.client.ui.Widget;
import com.google.gwt.view.client.SelectionChangeEvent;
import com.google.gwt.view.client.SingleSelectionModel;
import com.opendental.odweb.client.ui.ModuleWidget;
import com.opendental.odweb.client.usercontrols.OutlookBar;
import com.opendental.odweb.client.usercontrols.OutlookButton;

/** This is where the shell of the Open Dental Web App lives. */
public class WindowOpenDental extends ResizeComposite {
	private static WindowOpenDentalUiBinder uiBinder = GWT.create(WindowOpenDentalUiBinder.class);
	interface WindowOpenDentalUiBinder extends UiBinder<Widget, WindowOpenDental> {
	}
	
	/** The current {@link ModuleWidget} being displayed. */
  private ModuleWidget moduleCur;
  //Have a variable for each module so that we don't have to talk to the database for modules we have already loaded.
  private ModuleWidget contrAppt;
  private ModuleWidget contrFamily;
  private ModuleWidget contrAccount;
  private ModuleWidget contrTreatPlan;
  private ModuleWidget contrChart;
  private ModuleWidget contrImages;
  private ModuleWidget contrManage;
  /** Array list that contains the index of the selected module.  It might get enhanced to handle messaging buttons as well. */
	public ArrayList<Integer> selectedIndicies=new ArrayList<Integer>();
  /** The handler used to handle the user changing appointment views. */
  private HandlerRegistration apptViewSourceHandler;
  /** The panel that holds the content. */
  @UiField
  SimpleLayoutPanel contentPanel;
	/** The outlook bar on the left used to navigate to different modules. */
  @UiField(provided=true)
  OutlookBar outlookBar;
	
	public WindowOpenDental() {
		final SingleSelectionModel<OutlookButton> selectionModel=new SingleSelectionModel<OutlookButton>();
		outlookBar=new OutlookBar(selectionModel);
		//Create an event handler for when users click between modules.
		selectionModel.addSelectionChangeHandler(new SelectionChangeEvent.Handler() {
      public void onSelectionChange(SelectionChangeEvent event) {
        setModule(selectionModel.getSelectedObject().getButtonIndex());
      }
    });
    // Initialize the UI binder.
    initWidget(uiBinder.createAndBindUi(this));  
    //Default to the Appts module.
    setModule(0);
	}
	
	/** Set the module to display.
   * @param index The index of the module that needs to be displayed. */
  public void setModule(int index) {
    //Clear the old handler.
    if(apptViewSourceHandler!=null) {
    	apptViewSourceHandler.removeHandler();
    	apptViewSourceHandler=null;
    }
    moduleCur=getModuleAtIndex(index);
    if(moduleCur==null) {
    	//This is where we can disable the tool bar buttons and such when no patient is selected or the user logs off.
      contentPanel.setWidget(null);
      return;
    }
    //Setup the main tool bar here.
    // Show the widget.
    showModule();
  }

	private ModuleWidget getModuleAtIndex(int index) {
		switch(index) {
			case 0:
				if(contrAppt==null) {
					contrAppt=new ContrAppt();
				}					
				return contrAppt;
			case 1:
				if(contrFamily==null) {
					contrFamily=new ContrFamily();
				}
				return contrFamily;
			case 2:
				if(contrAccount==null) {
					contrAccount=new ContrAccount();
				}
				return contrAccount;
			case 3:
				if(contrTreatPlan==null) {
					contrTreatPlan=new ContrTreatPlan();
				}
				return contrTreatPlan;
			case 4:
				if(contrChart==null) {
					contrChart=new ContrChart();
				}
				return contrChart;
			case 5:
				if(contrImages==null) {
					contrImages=new ContrImages();
				}
				return contrImages;
			case 6:
				if(contrManage==null) {
					contrManage=new ContrManage();
				}
				return contrManage;
		}
		// TODO Handle messaging buttons here?
		return null;
	}

	private void showModule() {
		if(moduleCur==null) {
			return;
		}
		contentPanel.setWidget(moduleCur);
	}
	
	
	
}

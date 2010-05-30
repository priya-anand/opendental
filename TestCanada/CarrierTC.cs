﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenDentBusiness;

namespace TestCanada {
	public class CarrierTC {
		public static string SetInitialCarriers() {
			//We are starting with zero carriers
			Carrier carrier=new Carrier();
			carrier.IsCDA=true;
			carrier.CarrierName="Carrier 1";
			carrier.CanadianTransactionPrefix="CDANET14";
			carrier.CDAnetVersion="04";
			carrier.ElectID="666666";
			carrier.CanadianEncryptionMethod=2;
			carrier.CanadianSupportedTypes
				//claim_01 is implied
				= CanSupTransTypes.CobClaimTransaction_07
				//claimAck_11 is implied
				| CanSupTransTypes.ClaimAckEmbedded_11e
				//claimEob_21 is implied
				| CanSupTransTypes.ClaimEobEmbedded_21e
				| CanSupTransTypes.ClaimReversal_02
				| CanSupTransTypes.ClaimReversalResponse_12
				| CanSupTransTypes.PredeterminationSinglePage_03
				| CanSupTransTypes.PredeterminationMultiPage_03
				| CanSupTransTypes.PredeterminationAck_13
				| CanSupTransTypes.PredeterminationAckEmbedded_13e
				| CanSupTransTypes.RequestForOutstandingTrans_04
				| CanSupTransTypes.EmailTransaction_24
				| CanSupTransTypes.RequestForSummaryReconciliation_05
				| CanSupTransTypes.SummaryReconciliation_15;
			Carriers.Insert(carrier);
			//Carrier2---------------------------------------------------
			carrier=new Carrier();
			carrier.IsCDA=true;
			carrier.CarrierName="Carrier 2";
			carrier.CanadianTransactionPrefix="A";
			carrier.CDAnetVersion="04";
			carrier.ElectID="777777";
			carrier.CanadianEncryptionMethod=1;
			carrier.CanadianSupportedTypes
				= CanSupTransTypes.EligibilityTransaction_08
				| CanSupTransTypes.EligibilityResponse_18
				//claim_01 is implied
				//claimAck_11 is implied
				//claimEob_21 is implied
				| CanSupTransTypes.ClaimReversal_02
				| CanSupTransTypes.ClaimReversalResponse_12
				| CanSupTransTypes.PredeterminationSinglePage_03
				| CanSupTransTypes.PredeterminationAck_13
				| CanSupTransTypes.RequestForOutstandingTrans_04
				| CanSupTransTypes.EmailTransaction_24
				| CanSupTransTypes.RequestForPaymentReconciliation_06
				| CanSupTransTypes.PaymentReconciliation_16;
			Carriers.Insert(carrier);
			//Carrier3---------------------------------------------------
			carrier=new Carrier();
			carrier.IsCDA=true;
			carrier.CarrierName="Carrier 3";
			carrier.CanadianTransactionPrefix="AB";
			carrier.CDAnetVersion="04";
			carrier.ElectID="888888";
			carrier.CanadianEncryptionMethod=2;
			carrier.CanadianSupportedTypes
				= CanSupTransTypes.EligibilityTransaction_08
				| CanSupTransTypes.EligibilityResponse_18
				//claim_01 is implied
				| CanSupTransTypes.CobClaimTransaction_07
				//claimAck_11 is implied
				//claimEob_21 is implied
				| CanSupTransTypes.ClaimReversal_02
				| CanSupTransTypes.ClaimReversalResponse_12
				| CanSupTransTypes.PredeterminationSinglePage_03
				| CanSupTransTypes.PredeterminationAck_13
				| CanSupTransTypes.RequestForOutstandingTrans_04
				| CanSupTransTypes.EmailTransaction_24
				| CanSupTransTypes.RequestForPaymentReconciliation_06
				| CanSupTransTypes.PaymentReconciliation_16;
			Carriers.Insert(carrier);
			//Carrier4---------------------------------------------------
			carrier=new Carrier();
			carrier.IsCDA=true;
			carrier.CarrierName="Carrier 4";
			carrier.CanadianTransactionPrefix="ABC";
			carrier.CDAnetVersion="04";
			carrier.ElectID="999111";
			carrier.CanadianEncryptionMethod=2;
			carrier.CanadianSupportedTypes
				= CanSupTransTypes.EligibilityTransaction_08
				| CanSupTransTypes.EligibilityResponse_18
				//claim_01 is implied
				| CanSupTransTypes.CobClaimTransaction_07
				//claimAck_11 is implied
				//claimEob_21 is implied
				| CanSupTransTypes.ClaimReversal_02
				| CanSupTransTypes.ClaimReversalResponse_12
				| CanSupTransTypes.PredeterminationSinglePage_03
				| CanSupTransTypes.PredeterminationAck_13
				| CanSupTransTypes.RequestForOutstandingTrans_04
				| CanSupTransTypes.EmailTransaction_24
				| CanSupTransTypes.RequestForPaymentReconciliation_06
				| CanSupTransTypes.PaymentReconciliation_16;
			Carriers.Insert(carrier);
			//Carrier5---------------------------------------------------
			carrier=new Carrier();
			carrier.IsCDA=true;
			carrier.CarrierName="Carrier 5";
			carrier.CanadianTransactionPrefix="V2CAR";
			carrier.CDAnetVersion="02";
			carrier.ElectID="555555";
			carrier.CanadianEncryptionMethod=0;//not applicable
			carrier.CanadianSupportedTypes
				= CanSupTransTypes.EligibilityTransaction_08
				| CanSupTransTypes.EligibilityResponse_18
				//claim_01 is implied
				//claimAck_11 is implied
				//claimEob_21 is implied
				| CanSupTransTypes.ClaimReversal_02
				| CanSupTransTypes.ClaimReversalResponse_12
				| CanSupTransTypes.PredeterminationSinglePage_03
				| CanSupTransTypes.PredeterminationAck_13;
			Carriers.Insert(carrier);
			Carriers.RefreshCache();
			return "Carrier objects set.\r\n";
		}

		public static long GetCarrierNumById(string carrierId) {
			for(int i=0;i<Carriers.Listt.Length;i++) {
				if(Carriers.Listt[i].ElectID==carrierId) {
					return Carriers.Listt[i].CarrierNum;
				}
			}
			return 0;
		}

	}
}

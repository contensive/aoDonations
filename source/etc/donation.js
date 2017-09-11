
jQuery(document).ready(function() {
    jQuery("#js-donPayCheck").click(function() {
        // alert("Ready")
        jQuery("#js-payByCreditCardTR").hide();
        jQuery("#js-payByCheckTR").show();
    });
    //
    jQuery("#js-donAmt01").click(function() {
        jQuery("#donationAmount").val(5.00);
        //jQuery("#donationAmount").prop("disabled", true);
    });
    //
    jQuery("#js-donAmt02").click(function() {
        jQuery("#donationAmount").val(10.00);
        //jQuery("#donationAmount").prop("disabled", true);
    });
    //
    jQuery("#js-donAmt03").click(function() {
        jQuery("#donationAmount").val(15.00);
        //jQuery("#donationAmount").prop("disabled", true);
    });
    //
    jQuery("#js-donAmtOther").click(function() {
        jQuery("#donationAmount").val("");
        //jQuery("#donationAmount").prop("disabled", false);
    });
	//
	var form = $( "#donationInputForm" );
	form.validate();
	$("#subDonInput").click(function(e) {
        jQuery("#js-processError").hide();
		// alert( "Valid: " + form.valid() );
		if(form.valid() == false) {
			// alert('not valid');
			// e.preventDefault(); 
			jQuery.validator.setDefaults({
				debug: true,
				success: "valid"
			});
			$( "#donationInputForm" ).validate({
				rules: {
					DFFirstName: {
						required: true
					},
					DFcardYr: {
						required: true
					},
					DFcardType: {
						required: true
					},
					DFcardExp: {
						required: true
					}
				}
			});
		} else {
			jQuery('.donationForm').block();
			e.preventDefault(); 
			e.stopPropagation();
			cj.remote({
				'formId':'donationInputForm',
				'method': 'DonationFormHandler',
				'callback': donationCallback
			});
		}
	});
}); 
//*****************************************************
//donationCallback
//
function donationCallback(response) {
    //var json = (response);
	jQuery('.donationForm').unblock();
    var myObject = JSON.parse(response);
    if (myObject) {
        if (!myObject.ProcessedOk) {
			// processing error
			jQuery('#js-processError').html(myObject.errorMessage);
            jQuery("#js-donationConfirmation").hide();
            jQuery("#donationInputForm").show();
            jQuery("#js-processError").show();
		} else {
			// thank you
            jQuery("#donationInputForm").hide();
            jQuery("#js-donationConfirmation").show();
            if (myObject.name) {
                jQuery('#tyDonater').html(myObject.name);
            }
            if (myObject.completedDate) {
                jQuery('#tyDate').html(myObject.completedDate);
            }
            if (myObject.donationAmount) {
                jQuery('#tyAmount').html(myObject.donationAmount);
            }
            if (myObject.paymentHolderName) {
                jQuery('#tyPaymentInfo').html(myObject.paymentHolderName);
            }
			if (myObject.errorList)	{
				jquery('#js-errorList').html(myObject.errorList).show;
			}
			if (myObject.myDFPaymentMethod == '1'){
                jQuery('#tyPayMethod').html("Check")
            }else{
                jQuery('#tyPayMethod').html("Credit Card")
            };
			switch(myObject.donationType) {
				case 1:
					jQuery('#tyDonateType').html("Once");
					break;
				case 2:
					jQuery('#tyDonateType').html("Monthly");
					break;
				case 3:
					jQuery('#tyDonateType').html("Quarterly");
					break;
				case 4:
					jQuery('#tyDonateType').html("Annually");
					break;
				default:
					jQuery('#tyDonateType').html("Once");
			}
        }
   }
}

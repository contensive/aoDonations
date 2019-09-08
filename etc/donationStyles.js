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
	jQuery("#subDonInput").click(function(e) {
		try {
			e.preventDefault(); 
			e.stopPropagation();
			jQuery("#js-processError").hide();		
			var $FormList = $('#donationInputForm');
			var $donateForm = $FormList[0];
			// if(! $donateForm.checkValidity()) {
			// 	//
			// 	// If the form is invalid, submit it to display the native HTML5 error messages.
			// 	$donateForm.submit();
			// 	return;
			// }
			$.blockUI();
			jQuery.ajax({
				url: "/DonationFormHandler",
				data: jQuery("#donationInputForm").serialize(),
				success: function( response ) { donationCallback( response ) }
			})
		} catch( err ) {
			alert("Error submitting form, [" + err + "]");
		}
	});
}); 
/*****************************************************
* donationCallback
*/
function donationCallback( response ) {
    window.scrollTo(0, 0);
	$.unblockUI();
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

"use strict";

// Class Definition
var KTAddUser = function () {
	// Private Variables
	var _wizardEl;
	var _formEl;
	var _wizard;
	var _avatar;
	var _validations = [];

	// Private Functions
	var _initWizard = function () {
		// Initialize form wizard
		_wizard = new KTWizard(_wizardEl, {
			startStep: 1, // initial active step number
			clickableSteps: true  // allow step clicking
		});

		// Validation before going to next page
		_wizard.on('beforeNext', function (wizard) {
			// Don't go to the next step yet
			_wizard.stop();

			// Validate form
			var validator = _validations[wizard.getStep() - 1]; // get validator for currnt step
			validator.validate().then(function (status) {
		        if (status == 'Valid') {
					_wizard.goNext();
					KTUtil.scrollTop();
				} else {
					Swal.fire({
		                text: "با عرض پوزش ، به نظر می رسد برخی از خطاها شناسایی شده اند ، لطفا دوباره امتحان کنید.",
		                icon: "error",
		                buttonsStyling: false,
		                confirmButtonText: "باشه فهمیدم!",
						customClass: {
							confirmButton: "btn font-weight-bold btn-light"
						}
		            }).then(function() {
						KTUtil.scrollTop();
					});
				}
		    });
		});

		// Change Event
		_wizard.on('change', function (wizard) {
			KTUtil.scrollTop();
		});
	}

	var _initValidations = function () {
		// Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/

		// Validation Rules For Step 1
		_validations.push(FormValidation.formValidation(
			_formEl,
			{
				fields: {
					firstname: {
						validators: {
							notEmpty: {
								message: 'First Name is required'
							}
						}
					},
					lastname: {
						validators: {
							notEmpty: {
								message: "نام خانوادگی لازم است"
							}
						}
					},
					companyname: {
						validators: {
							notEmpty: {
								message: 'Company Name is required'
							}
						}
					},
					phone: {
						validators: {
							notEmpty: {
								message: "تلفن لازم است"
							},
							phone: {
								country: 'US',
								message: "این تعداد شماره تلفن معتبر ایالات متحده نیست. (به عنوان مثال 5554443333) "
							}
						}
					},
					email: {
						validators: {
							notEmpty: {
								message: 'ایمیل لازم است'
							},
							emailAddress: {
								message: "مقدار یک آدرس ایمیل معتبر نیست"
							}
						}
					},
					companywebsite: {
						validators: {
							notEmpty: {
								message: 'آدرس وب سایت لازم است'
							}
						}
					}
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					bootstrap: new FormValidation.plugins.Bootstrap()
				}
			}
		));

		_validations.push(FormValidation.formValidation(
			_formEl,
			{
				fields: {
					// Step 2
					communication: {
						validators: {
							choice: {
								min: 1,
								message: "لطفا حداقل 1 گزینه را انتخاب کنید"
							}
						}
					},
					language: {
						validators: {
							notEmpty: {
								message: "لطفا یک زبان را انتخاب کنید"
							}
						}
					},
					timezone: {
						validators: {
							notEmpty: {
								message: "لطفا منطقه زمانی را انتخاب کنید"
							}
						}
					}
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					bootstrap: new FormValidation.plugins.Bootstrap()
				}
			}
		));

		_validations.push(FormValidation.formValidation(
			_formEl,
			{
				fields: {
					address1: {
						validators: {
							notEmpty: {
								message: "آدرس لازم است"
							}
						}
					},
					postcode: {
						validators: {
							notEmpty: {
								message: "کدپستی لازم است"
							}
						}
					},
					city: {
						validators: {
							notEmpty: {
								message: "شهر مورد نیاز است"
							}
						}
					},
					state: {
						validators: {
							notEmpty: {
								message: "استان لازم است"
							}
						}
					},
					country: {
						validators: {
							notEmpty: {
								message: "کشور مورد نیاز است"
							}
						}
					},
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					bootstrap: new FormValidation.plugins.Bootstrap()
				}
			}
		));
	}

	var _initAvatar = function () {
		_avatar = new KTImageInput('kt_user_add_avatar');
	}

	return {
		// public functions
		init: function () {
			_wizardEl = KTUtil.getById('kt_wizard');
			_formEl = KTUtil.getById('kt_form');

			_initWizard();
			_initValidations();
			_initAvatar();
		}
	};
}();

jQuery(document).ready(function () {
	KTAddUser.init();
});

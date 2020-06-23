

var app = angular.module('PendingSRVCloserModule', []);

app.service('PendingSRVCloserService', function ($http, $location, HomeService) {


    this.UpdateRequestbyCSO = function (Obj) {

        return $http.post(HomeService.AppUrl + 'Request/UpdateRequestbyCSO', Obj, {});

    };
    this.UpdateDONo = function (Obj) {

        return $http.post(HomeService.AppUrl + 'Request/UpdateDONo', Obj, {});

    };
    this.AckbyCommercial = function (Obj) {

        return $http.post(HomeService.AppUrl + 'Request/AckbyCommercial', Obj, {});
    };

    this.ApprovebyDepot = function (Obj) {

        return $http.post(HomeService.AppUrl + 'Request/ApprovebyDepot', Obj, {});

    };

    this.getRequestforBillingClosure = function (EmpCode) {

        return $http.get(HomeService.AppUrl + 'RequestType/getRequestforBillingClosure?EmpCode=' + EmpCode, {});
    };

    this.GetSAPReasonDetailByReason = function (ReqType) {

        return $http.get(HomeService.AppUrl + 'RequestType/GetSAPReasonDetailByReason?ReqType=' + ReqType, {});

    };
    this.AcknowledgeRequestbyDepot = function (Obj) {
        return $http.post(HomeService.AppUrl + 'Request/AcknowledgeRequestbyDepot', Obj, {});
    };

    this.GetRequestDetailsforNextStage = function (Id, CurrentStatus_Id, FutureStatus_Id) {

        return $http.get(HomeService.AppUrl + 'Common/GetRequestDetailsforNextStage?RequestId=' + Id + '&CurrentStatus_Id=' + CurrentStatus_Id + '&FutureStatus_Id=' + FutureStatus_Id, {});

    };

    this.getSubReasonList = function (ReasonId) {
        return $http.get('api/RequestType/GetSubReasonList?Id=' + ReasonId, {});
    };
    this.ReconsiderByCommercial = function (Obj) {
        return $http.post(HomeService.AppUrl + 'Request/ReconsiderByCommercial', Obj, {});
    }
});

app.controller('PendingSRVCloserController', function ($scope, HomeService, $uibModal, RequestTypeService, CSOReasonService, ApprovedRequestService, ReasonService, PendingRequestService, SRVCloserService, PendingSRVCloserService,$rootScope, SalesReturnsService) {
    console.log("AdminMaster");
    $scope.AdminList = [];

    $scope.init = function () {
        //if($rootScope.session.DepotId==0)
        PendingSRVCloserService.getRequestforBillingClosure($rootScope.session.EMP_CODE).then(function (success) {
            $scope.showView = false;
            $scope.ApprovedRequestList = success.data;

        }, function (error) {
            console.log(error);
        });

        //CSOReasonService.GetSAPReasonDetailByReason().then(function (success) {
        //    console.log(success);
        //    $scope.ReasonDeatilForSap = success.data;

        //});

    };

    $scope.ViewRequestDetails = function (Id, CurrentStatus_Id, FutureStatus_Id, Obj) {
        $scope.Remarks = '';
        SalesReturnsService.GetSKUCode().then(function (success) {
            $scope.SKUCodeList = success.data;
            $scope.Request = {
                RequestDetail: [{
                    InvoiceQuantity: 0,
                    PackSize: 0
                }]
            };
            return SalesReturnsService.GetDepotList()
        }).then(function (success) {

            if (success.data != null) {
                $scope.DepotList = success.data;

            }
            return RequestTypeService.GetRequestTypeList();
        }).then(function (success) {

            $scope.RequestTypeList = success.data;
            return SalesReturnsService.GetCustomerComplaintList();
        }).then(function (success) {
            $scope.CCList = success.data;
            return ReasonService.GetReasonDetail();
        }).then(function (success) {
            $scope.SubReason = success.data;

        //    return SRVCloserService.GetSAPReasonDetailByReason($scope.ApprovedRequestDetails.ReasonForReturn_Id);
        //}).then(function (success) {
        //    $scope.SAPReasonList = success.data;

        }, function (error) {
            console.log(error);

        });

        $scope.showView = true;

        $scope.CurrentStatus_Id = CurrentStatus_Id;
        $scope.Request_Id = Id;
        $scope.FutureStatus_Id = FutureStatus_Id;


        //Display purpose only

        $scope.TotalSRV_ = Obj.TotalSRV;
        $scope.CreatedDate_ = Obj.CreatedDate;
        $scope.CreatedBy_ = Obj.CreatedBy + ":" + Obj.CreatedBy_EMP_CODE;

        PendingSRVCloserService.GetRequestDetailsforNextStage(Id, CurrentStatus_Id, FutureStatus_Id).then(function (success) {

            $scope.ApprovedRequestGrid = false;
            $scope.ApprovedRequestForm = true;

            $scope.ApprovedRequestDetails = success.data;
            console.log($scope.ApprovedRequestDetails);

            //angular.forEach($scope.ApprovedRequestDetails.RequestDetail, function (value, key) {
            //    value.Damaged = 0;
            //}); 

            PendingSRVCloserService.GetSAPReasonDetailByReason($scope.ApprovedRequestDetails.ReasonForReturn_Id).then(function (success) {
                $scope.SAPReasonList = success.data;
                return PendingSRVCloserService.getSubReasonList($scope.ApprovedRequestDetails.ReasonForReturn_Id);
            }).then(function (success) {

                $scope.SubReasonList = success.data;
            }, function (error) {
                console.log(error);
            });
        },
            function (error) {
            console.log(error);
            });

        HomeService.GetRequestStatusDetails(Id).then(function success(retdata) {
            //debugger
            if (retdata != null) {
                $scope.grid = retdata.data;
            }
            else {
                console.log('Error case');
            }
        }, function error(retdata) {

        });

    };

    $scope.ReadRecvdQuantity = function () {

        angular.forEach($scope.ApprovedRequestDetails.RequestDetail, function (value, key) {
            value.Short = value.SRVQuantity - value.ReceivedQuantity;
            value.Excess = value.ReceivedQuantity - value.SRVQuantity;
        });

    };

    //$scope.ReadRecvdQuantity = function () {

    //    angular.forEach($scope.ApprovedRequestDetails.RequestDetail, function (value, key) {

    //        value.Short = value.SRVQuantity - value.ReceivedQuantity;
    //        value.Excess = value.ReceivedQuantity - value.ReceivedQuantity;
    //    });
    //}

    $scope.CompareDate = function (invoiceDate) {

        if (invoiceDate) {
            var d1 = new Date('2017-07-31');
            var d2 = new Date(invoiceDate);

            if (d2 > d1) {
                return false;
            }
            else {
                return true;
            }
        }

    };

    $scope.openStatus = function (id, reqoption) {
        //$scope.items.IndexId = _Item;
        console.log("reqId", id);

        HomeService.RequestID = id;
        HomeService.requestTypeoption = reqoption;
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'Partial/MyModelPopUp.html',
            controller: 'ModalInstanceCtrl_StatusPending',
            size: 'lg',
            scope: $scope,
            resolve: {
                items: function () {
                    return id;
                }
            }
        });

    };

    $scope.AcknowledgeByDepot = function (ReqDetail) {

        console.log($scope.ApprovedRequestDetails);
        var check = true;

        angular.forEach($scope.ApprovedRequestDetails.RequestDetail, function (value, key) {

            if (value.ReceivedQuantity == null || value.ReceivedQuantity == "") {
                swal("Please Enter the Receiving Quantity");
                check = false;
                return false;
            }
            //if (value.SAPsubReasonID == null || value.SAPsubReasonID == "") {
            //    swal("Please select the SAP Sub Reason");
            //    check = false;
            //    return false;
            //}
            if ((value.ReceivedQuantity / 1) > (value.InvoiceQuantity / 1)) {
                swal("Received Quantity can not be greater than Invoice  Quantity");
                check = false;
                return false;
            }
            if ((value.Damaged / 1) > (value.ReceivedQuantity / 1)) {
                swal("Damaged Quantity can not be greater than Received Quantity");
                check = false;
                return false;
            }
            if ((value.Damaged / 1) > (value.InvoiceQuantity / 1)) {
                swal("Damaged Quantity can not be greater than Invoice Quantity");
                check = false;
                return false;
            }
            value.Acknowledge = true;
        });
        if (check == true) {

            console.log($scope.CurrentStatus_Id);
            console.log($scope.Request_Id);

            var CurrentStatus_Id = 0;
            var FutureStatus_Id = 0;
            var Active_Role = 0;
            var Requested_Role = 0;

            if ($scope.CurrentStatus_Id == 16) {
                CurrentStatus_Id = 10022;//acknowledge by Depot
                FutureStatus_Id = 10020;//Pending CSO
                Active_Role = 10; //depot
                Requested_Role = 11; // cso
            }

            swal({
                title: "Are you sure?",
                text: "You want to update this request?",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes",
                closeOnConfirm: false
            },
                function () {
                    var Obj = {

                        EmployeeCode: $rootScope.session.EMP_CODE,
                        Country: $rootScope.session.Country,

                        CurrentStatus_Id: CurrentStatus_Id,
                        FutureStatus_Id: FutureStatus_Id,

                        Request_Id: $scope.Request_Id,
                        ReasonForReturn_Id: ReqDetail.ReasonForReturn_Id,

                        Active_Role: Active_Role,
                        Requested_Role: Requested_Role,
                        Remarks: $scope.Remarks,
                        RequestDetail: $scope.ApprovedRequestDetails.RequestDetail
                    };
                    console.log(Obj);

                    PendingSRVCloserService.AcknowledgeRequestbyDepot(Obj).then(function (success) {
                        $scope.init();
                        swal("Success", success.data, "success");

                    }, function (error) {
                        console.log(error);
                    });
                });
        };
    };

    $scope.AddNewRow = function () {

        $scope.AdminList.push({

            EmployeeCode: ""
        });

    }; 

    $scope.ackByCommercial = function (ReqDetail) {
        var check = true;
        var ErrorMsg = '';
        console.log($scope.ApprovedRequestDetails);

        console.log($scope.CurrentStatus_Id);
        console.log($scope.Request_Id);
        //  console.log(RequestType_Id);
        angular.forEach($scope.ApprovedRequestDetails.RequestDetail, function (val, key) {
            if ((!val.DONo && val.ReleaseByCM) || (val.DONo && !val.ReleaseByCM)) { ErrorMsg='Please Release The DO number!'; check = false;  }
        });
       
        if (check == false) {
            swal(ErrorMsg);
            return false;
        }
        else
        {
            if ($scope.Remarks == "" || $scope.Remarks == null) {
                //check = false;
                swal("Please Enter Remarks");
                return false;
            }
            var CurrentStatus_Id = 0;
            var FutureStatus_Id = 0;
            var Active_Role = 0;
            var Requested_Role = 0;

            if ($scope.CurrentStatus_Id == 10031) {
                CurrentStatus_Id = 10024;//acknowledge by Commercial
                FutureStatus_Id = 10019;//Pending depot
                Active_Role = 12; //Commercial
                Requested_Role = 10; // cso
            }
            //if ($scope.CurrentStatus_Id == 10024) {


            //    CurrentStatus_Id = 10025;//approve by Depot
            //    FutureStatus_Id = NULL;
            //    Active_Role = 10; //depot
            //    Requested_Role = 0;

            //}

            swal({
                title: "Are you sure?",
                text: "You want to Approve this request?",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes",
                closeOnConfirm: false
            },
                function () {
                    var Obj = {

                        EmployeeCode: $rootScope.session.EMP_CODE,
                        Country: $rootScope.session.Country,

                        CurrentStatus_Id: CurrentStatus_Id,
                        FutureStatus_Id: FutureStatus_Id,

                        Request_Id: $scope.Request_Id,
                        ReasonForReturn_Id: ReqDetail.ReasonForReturn_Id,

                        Active_Role: Active_Role,
                        Requested_Role: Requested_Role,
                        Remarks: $scope.Remarks,
                        DONo: $scope.DONo,
                        RequestDetail: $scope.ApprovedRequestDetails.RequestDetail
                    };

                    console.log(Obj);

                    PendingSRVCloserService.AckbyCommercial(Obj).then(function (success) {

                        $scope.init();

                        swal("Success", success.data, "success");

                    }, function (error) {
                        console.log(error);
                    });
                });
        }
    };

    $scope.ApproveByDepot = function (ReqDetail) {
        var check = true;

        console.log($scope.ApprovedRequestDetails);

        angular.forEach($scope.ApprovedRequestDetails.RequestDetail, function (value, key) {


            if (value.SRVInvoiceNo == null || value.SRVInvoiceNo == "") {
                check = false;
                swal("Please Enter the SRV Invoice No.");
                return false;
            }



        });

        console.log($scope.CurrentStatus_Id);
        console.log($scope.Request_Id);
        //  console.log(RequestType_Id);

        if ($scope.Remarks == "" || $scope.Remarks == null) {
            check = false;
            swal("Please Enter Remarks");
            return false;
        }

        if (check == false) {
            swal("Please Enter the SRV Invoice No.");
            return false;
        } else {



            var CurrentStatus_Id = 0;
            var FutureStatus_Id = 0;
            var Active_Role = 0;
            var Requested_Role = 0;

          
            if ($scope.CurrentStatus_Id == 10024) {


                CurrentStatus_Id = 10025;//approve by Depot
                FutureStatus_Id = 0;
                Active_Role = 10; //depot
                Requested_Role = 0;

            }



            swal({
                title: "Are you sure?",
                text: "You want to Approve this request?",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes",
                closeOnConfirm: false
            },
                function () {
                    var Obj = {

                        EmployeeCode: $rootScope.session.EMP_CODE,
                        Country: $rootScope.session.Country,

                        CurrentStatus_Id: CurrentStatus_Id,
                        FutureStatus_Id: FutureStatus_Id,

                        Request_Id: $scope.Request_Id,
                        ReasonForReturn_Id: ReqDetail.ReasonForReturn_Id,

                        Active_Role: Active_Role,
                        Requested_Role: Requested_Role,
                        Remarks: $scope.Remarks,
                        DONo: $scope.DONo,
                        RequestDetail: $scope.ApprovedRequestDetails.RequestDetail

                    };

                    console.log(Obj);

                    PendingSRVCloserService.ApprovebyDepot(Obj).then(function (success) {

                        $scope.init();

                        swal("Success", success.data, "success");

                    }, function (error) {
                        console.log(error);
                    });


                });

        }

    };

    $scope.updateDetail = function (ReqDetail) {
        var check = true;
        
        angular.forEach($scope.ApprovedRequestDetails.RequestDetail, function (value, key) {

            if (!value.SAPsubReasonID) {
                check = false;
                swal("Please Select SAP Reason");
                return false;
            }
        });

        if (check==false) {
            swal("Please Select SAP Reason");
            return false;
        } else {
            if ($scope.Remarks == "" || $scope.Remarks == null) {
                swal("Please Enter Remarks");
                return false;
            }
        var CurrentStatus_Id = 0;
        var FutureStatus_Id = 0;
        var Active_Role = 0;
            var Requested_Role = 0;
            //CurrentStatus_Id = 10023;//acknowledge by CSO
            //FutureStatus_Id = 10021;//Pending Commercial
            //Active_Role = 11; //cso
            //Requested_Role = 12; // commercial
        if ($scope.CurrentStatus_Id == 10022) {
            CurrentStatus_Id = 10023;//acknowledge by CSO
            FutureStatus_Id = 10019;//Pending Commercial
            Active_Role = 11; //cso
            Requested_Role = 10; // commercial
            }
            if ($scope.CurrentStatus_Id == 10023) {

                CurrentStatus_Id = 10024;//acknowledge by Commercial
                FutureStatus_Id = 10019;//Pending depot
                Active_Role = 12; //depot
                Requested_Role = 10; // cso
            }
            if ($scope.CurrentStatus_Id == 10024) {
                CurrentStatus_Id = 10025;//approve by Depot
                FutureStatus_Id = NULL;
                Active_Role = 10; //depot
                Requested_Role = 0; 
            }

        swal({
            title: "Are you sure?",
            text: "You want to Approve this request?",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Yes",
            closeOnConfirm: false
        },
            function () {
                var Obj = {

                    EmployeeCode: $rootScope.session.EMP_CODE,
                    Country: $rootScope.session.Country,
                    DepotId: $scope.ApprovedRequestDetails.DepotId,
                    DealerId: $scope.ApprovedRequestDetails.DealerId,
                    CurrentStatus_Id: CurrentStatus_Id,
                    FutureStatus_Id: FutureStatus_Id,

                    Request_Id: $scope.Request_Id,
                    ReasonForReturn_Id: ReqDetail.ReasonForReturn_Id,

                    Active_Role: Active_Role,
                    Requested_Role: Requested_Role,
                    Remarks: $scope.Remarks,
                    DONo: $scope.DONo,
                    RequestDetail: $scope.ApprovedRequestDetails.RequestDetail

                };

                console.log(Obj);
                PendingSRVCloserService.UpdateRequestbyCSO(Obj).then(function (success) {
                    if (success.data.indexOf('Error') != -1) {
                        swal("Error", success.data, "error");
                    }
                    else {
                        $scope.init();
                        swal("Success", success.data, "success");
                    }
                }, function (error) {
                    console.log(error);
                });
            });
        }
    };

    $scope.updateDoNumber = function (ReqDetail) {
        var check = true;
        angular.forEach($scope.ApprovedRequestDetails.RequestDetail, function (value, key) {
            if (!value.DONo) {
                check = false;
                swal("Please Enter Do Number");
                return false;
            }
        });

        if (check == false) {
            swal("Please Enter Do Number");
            return false;
        }
        else
        {
            if ($scope.Remarks == "" || $scope.Remarks == null) {
                swal("Please Enter Remarks");
                return false;
            }
            var CurrentStatus_Id = 0;
            var FutureStatus_Id = 0;
            var Active_Role = 0;
            var Requested_Role = 0;

            //CurrentStatus_Id = 10023;//acknowledge by CSO
            //FutureStatus_Id = 10021;//Pending Commercial
            //Active_Role = 11; //cso
            //Requested_Role = 12; // commercial
            
            if ($scope.CurrentStatus_Id == 10023 || $scope.CurrentStatus_Id == 10032) {

                CurrentStatus_Id = 10031;//acknowledge by Commercial
                FutureStatus_Id = 10021;//Pending depot
                Active_Role = 10; //depot
                Requested_Role = 12; // cso
            }

            swal({
                title: "Are you sure?",
                text: "You want to Approve this request?",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes",
                closeOnConfirm: false
            },
                function () {
                    var Obj = {
                        EmployeeCode: $rootScope.session.EMP_CODE,
                        Country: $rootScope.session.Country,
                        DepotId: $scope.ApprovedRequestDetails.DepotId,
                        DealerId: $scope.ApprovedRequestDetails.DealerId,
                        CurrentStatus_Id: CurrentStatus_Id,
                        FutureStatus_Id: FutureStatus_Id,

                        Request_Id: $scope.Request_Id,
                        ReasonForReturn_Id: ReqDetail.ReasonForReturn_Id,

                        Active_Role: Active_Role,
                        Requested_Role: Requested_Role,
                        Remarks: $scope.Remarks,
                        DONo: $scope.DONo,
                        RequestDetail: $scope.ApprovedRequestDetails.RequestDetail
                    };

                    console.log(Obj);
                    PendingSRVCloserService.UpdateDONo(Obj).then(function (success) {
                        $scope.init();
                        swal("Success", success.data, "success");
                    }, function (error) {
                        console.log(error);
                    });
                });
        }
    };

    $scope.ReconsiderByCommercial = function (ApprovedRequestDetails) {

        if ($scope.Remarks == "" || $scope.Remarks == null) {
            swal("Warning", "Please Enter Remarks", "warning");
            return false;
        }
        else
        {

            var CurrentStatus_Id = 10032; //Reconsider User
            var FutureStatus_Id = 10019; //Pending User
            var Active_Role = 12;
            var Requested_Role = 10;

            var Obj = {
                EmployeeCode: $rootScope.session.EMP_CODE,
                Country: $rootScope.session.Country,

                CurrentStatus_Id: CurrentStatus_Id,
                FutureStatus_Id: FutureStatus_Id,

                Request_Id: $scope.Request_Id,
                RequestType_Id: ApprovedRequestDetails.RequestType_Id,

                Active_Role: Active_Role,
                Requested_Role: Requested_Role,
                Remarks: $scope.Remarks
            };
            swal({
                title: "Are you sure?",
                text: "You are going to Reconsider the request!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#0373fc", confirmButtonText: "Yes",
                cancelButtonText: "No",
                closeOnConfirm: false,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        PendingSRVCloserService.ReconsiderByCommercial(Obj).then(function (success) {
                            $scope.init();
                            swal("Success", success.data, "success");
                        }, function (error) {
                            swal("Error", error.data, "error");
                            console.log(error);
                        });
                    } else {
                        //swal("Cancelled", "Your imaginary file is safe :)", "error");
                    }
                });
        }
    };

    $scope.init();
});

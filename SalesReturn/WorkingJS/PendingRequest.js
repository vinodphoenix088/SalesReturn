var app = angular.module('PendingRequestModule', []);

app.service('PendingRequestService', function ($http, $location, HomeService) {

    this.getPendingRequest = function (EmployeeCode) {

        return $http.get(HomeService.AppUrl + 'PendingRequest/getPendingRequest?EmployeeCode=' + EmployeeCode, {});
    };

    this.RejectPendingRequest = function (Id, EmployeeCode) {

        return $http.get(HomeService.AppUrl + 'PendingRequest/RejectPendingRequest?Id=' + Id + '&EmployeeCode=' + EmployeeCode, {});
    };

    this.GetRequestDetails = function (Id, CurrentStatus_Id, FutureStatus_Id) {

        return $http.get(HomeService.AppUrl + 'Common/GetRequestDetails?RequestId=' + Id + '&CurrentStatus_Id=' + CurrentStatus_Id + '&FutureStatus_Id=' + FutureStatus_Id, {});
    };

    this.RecommendRequest = function (Obj) {

        return $http.post(HomeService.AppUrl + 'PendingRequest/RecommendRequest', Obj, {});
    };
    this.CloseRequest = function (Obj) {

        return $http.post(HomeService.AppUrl + 'PendingRequest/CloseRequest', Obj, {});
    };
    this.RejectRequest = function (Obj) {

        return $http.post(HomeService.AppUrl + 'PendingRequest/RejectRequest', Obj, {});
    };

    this.ApproveRequest = function (Obj) {

        return $http.post(HomeService.AppUrl + 'PendingRequest/ApproveRequest', Obj, {});
    };

    this.ReconsiderRequest = function (Obj) {

        return $http.post(HomeService.AppUrl + 'PendingRequest/ReconsiderRequest', Obj, {});
    };

});

app.controller('PendingRequestController', function ($scope, HomeService, $filter, SalesReturnsService, RequestTypeService, $uibModal, PendingRequestService, $rootScope) {
    console.log("PendingRequest");
    $scope.TodayDate = new Date();
    $scope.PendingRequestList = [];

    $scope.init = function () {
        $scope.IsReconsidered = false;

        console.log($rootScope.session);

        $scope.PendingRequestGrid = true;
        $scope.PendingRequestForm = false;

        PendingRequestService.getPendingRequest($rootScope.session.EMP_CODE).then(function (success) {

            $scope.PendingRequestList = success.data;


            return SalesReturnsService.GetSKUCode();
        }).then(function (success) {

            $scope.SKUCodeList = success.data;



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
            return SalesReturnsService.GetLastThreeMonthDate();
        }).then(function (success) {

            $scope.LastDate = success.data;
        }, function (error) {
            console.log(error);


        });

    };

    $scope.ResetProvideGST = function (index) {
        console.log(index);
        $scope.PendingRequestDetails.RequestDetail[index].ProvideGST_No = false;
        $scope.PendingRequestDetails.RequestDetail[index].ProvideGST_Yes = false;
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

    $scope.GetDealerList = function (DepotId) {


        SalesReturnsService.GetDealerList(DepotId).then(function (success) {

            if (success.data != null) {
                $scope.DealerList = success.data;
                $scope.PendingRequestDetails.DealerObj = $scope.DealerList.find(x => x.DealerId == $scope.PendingRequestDetails.DealerId);

            }

        }, function (error) {

            console.log(error);

        });

    };

    $scope.ViewRequestDetails = function (Id, CurrentStatus_Id, FutureStatus_Id, Obj) {

        $scope.CurrentStatus_Id = CurrentStatus_Id;
        $scope.Request_Id = Id;
        $scope.FutureStatus_Id = FutureStatus_Id;

        $scope.TotalSRV_ = Obj.TotalSRV;
        $scope.CreatedDate_ = Obj.CreatedDate;
        $scope.CreatedBy_ = Obj.CreatedBy + " : " + Obj.CreatedBy_EMP_CODE;

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

        PendingRequestService.GetRequestDetails(Id, CurrentStatus_Id, FutureStatus_Id).then(function (success) {

            $scope.PendingRequestGrid = false;
            $scope.PendingRequestForm = true;

            $scope.PendingRequestDetails = success.data;

            angular.forEach($scope.PendingRequestDetails.RequestDetail, function (value, key) {
                SalesReturnsService.GetBatchNO(value.SKUCode).then(function (success) {
                    value.BatchList = success.data;
                });
            });

            SalesReturnsService.getSubReasonList($scope.PendingRequestDetails.ReasonForReturn_Id).then(function (success) {
                $scope.SubReasonList = success.data;
            }, function (error) {
                console.log(error);
            });

            if (CurrentStatus_Id == 1) {
                $scope.Level_1 = true;
                $scope.Level_2 = false;
            }

            else
            //if (CurrentStatus_Id == 10 || CurrentStatus_Id == 11 || CurrentStatus_Id == 12 || CurrentStatus_Id == 13)
            {
                $scope.Level_1 = false;
                $scope.Level_2 = true;
            }

            if (FutureStatus_Id == 6 || FutureStatus_Id == 7 || FutureStatus_Id == 8) {

                $scope.Level_1 = false;
                $scope.Level_2 = true;
            }

            if (FutureStatus_Id == 17 && Obj.CreatedBy_EMP_CODE == $rootScope.session.EMP_CODE) {
                $scope.IsReconsidered = true;
                $scope.GetDealerList($scope.PendingRequestDetails.DepotId);

                angular.forEach($scope.PendingRequestDetails.RequestDetail, function (value, key) {

                    value.InvoiceDate = $filter('date')(value.InvoiceDate, "dd MMM yyyy");
                });

                $scope.PendingRequestDetails.DepotObj = $scope.DepotList.find(x => x.DepotId == $scope.PendingRequestDetails.DepotId);

                $scope.PendingRequestDetails.ReasonForReturnObj = $scope.RequestTypeList.find(x => x.RequestType_Id == $scope.PendingRequestDetails.ReasonForReturn_Id);

                console.log($scope.PendingRequestDetails);
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.UpdateRequest = function () {

        console.log($scope.PendingRequestDetails);

        $scope.PendingRequestDetails.DepotId = $scope.PendingRequestDetails.DepotObj.DepotId;
        $scope.PendingRequestDetails.DealerId = $scope.PendingRequestDetails.DealerObj.DealerId;
        $scope.PendingRequestDetails.ReasonForReturn_Id = $scope.PendingRequestDetails.ReasonForReturnObj.RequestType_Id;
        angular.forEach($scope.PendingRequestDetails.RequestDetail, function (value, key) {
            angular.forEach($scope.PendingRequestDetails.RequestDetail, function (value1, key1) {
                if (value1.InvoiceNo == value.InvoiceNo && value1.selectedSKU.SKUCode == value.selectedSKU.SKUCode && value1.selectedSKU.Batch_No == value.selectedSKU.Batch_No) {
                    swal("Error","Duplicate Invoice Details are not allowed!","error");
                    return false;
                };
            });
        });
        var check = true;
        if (!$scope.PendingRequestDetails.DepotId) {

            swal("Please select Depot");
            return false;
        }
        else if (!$scope.PendingRequestDetails.DealerId) {

            swal("Please select Dealer");
            return false;
        }
        else if (!$scope.PendingRequestDetails.ReasonForReturn) {

            swal("Please select Reason for return   ");
            return false;
        }

        angular.forEach($scope.PendingRequestDetails.RequestDetail, function (value, key) {

            var d1 = new Date($scope.LastDate);
            var d2 = new Date(value.invoiceDate);
            console.log(d1);
            console.log(d2);

            if (!value.InvoiceNo) {

                swal("Please enter invoice number");
                check = false;
                return false;
            }
            else if (!value.InvoiceDate) {

                swal("Please enter invoice date");
                check = false;
                return false;

            } else if (!value.ProvideGST_Yes && !value.ProvideGST_No && (d2 < d1)) {

                swal("Please select checkbox for providing GST invoice");
                check = false;
                return false;
            } else if (value.ProvideGST_Yes && !value.InvoiceUpload && !value.UploadedInvoice) {

                swal("Please upload invoice copy");
                check = false;
                return false;
            }

            else if (!value.selectedSKU) {

                swal("Please enter SKU code");
                check = false;
                return false;

            } else if (!value.BatchNo) {

                swal("Please enter Batch number");
                check = false;
                return false;

            } else if (!value.InvoiceQuantity) {

                swal("Please enter invoice quantity");
                check = false;
                return false;

            } else if (!value.SRVQuantity) {

                swal("Please enter SRV quantity");
                check = false;
                return false;

            } else if (!value.Unit) {

                swal("Please enter Unit");
                check = false;
                return false;
            } else if (!value.PackSize) {

                swal("Please enter Pack Size");
                check = false;
                return false;
            }

            else if (!value.SRVValue) {

                swal("Please enter SRV Value");
                check = false;
                return false;
            }
            else if ($scope.PendingRequestDetails.ReasonForReturn_Id == 1 && !value.selectedComplaint) {

                swal("Please enter Complaint number");
                check = false;
                return false;
            }
            else if (!value.Remarks) {

                swal("Please enter Remarks");
                check = false;
                return false;
            }
        });

        if (check == true) {
            console.log($scope.PendingRequestDetails);
            var fd = new FormData();
            var i = 0;
            angular.forEach($scope.PendingRequestDetails.RequestDetail, function (value, key) {
                console.log(value);
                if (value.ProvideGST_Yes) {
                    if (value.UploadedInvoice && value.InvoiceUpload) {
                        value.UploadedInvoice = null;
                        console.log("for null");
                    }
                    fd.append(i + '_1', value.InvoiceUpload);
                    i++;
                }
                value.selectedSKU.Batch_No = value.BatchNo;
                value.selectedSKU.Manufacturing_Date = value.Manufacturing_Date;
                value.selectedSKU.PackSize = value.PackSize;
                value.selectedSKU.Shelf_Life = value.Shelf_Life;
                value.selectedSKU.Unit = value.Unit;
                value.SubreasonId = value.SubReason;

            });
            console.log(i);

            $scope.PendingRequestDetails.EmployeeCode = $rootScope.session.EMP_CODE;
            $scope.PendingRequestDetails.Country = $rootScope.session.Country;

            $scope.CopyRequestDetail = [];
            var k = true;
            console.log($scope.PendingRequestDetails);
            angular.forEach($scope.PendingRequestDetails.RequestDetail, function (value, key) {
                console.log("$scope.PendingRequestDetails");

                angular.forEach($scope.CopyRequestDetail, function (value1, key1) {

                    if (value1.InvoiceNo == value.InvoiceNo && value1.selectedSKU.SKUCode == value.selectedSKU.SKUCode && value1.selectedSKU.Batch_No == value.selectedSKU.Batch_No) {

                        k = false;
                    };
                });

                $scope.CopyRequestDetail.push(value);
            });


            if (k == false) {
                swal("error", "The combination of Invoice no, SKU Name and Batch no should not be same.", "error");
                return false;
            }
            else {

                console.log($scope.PendingRequestDetails);
                fd.append('data', angular.toJson($scope.PendingRequestDetails));

                swal({
                    title: "Are you sure?",
                    text: "You are going to update request!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#0373fc", confirmButtonText: "Yes",
                    cancelButtonText: "No",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            SalesReturnsService.saveRequest(fd).then(function (success) {
                                console.log(success.data);
                                if (success.data.indexOf("Success") != -1) {
                                    swal("Success", success.data, "success");

                                    $scope.init();
                                }
                                else {
                                    swal("Error", success.data, "error");
                                }
                            });
                        } else {
                            //swal("Cancelled", "Your imaginary file is safe :)", "error");
                        }
                    });
            }
        }
    };

    $scope.GetBatchNO = function (obj, index) {

        //angular.forEach($scope.PendingRequestDetails.RequestDetail, function (value, key) {

        SalesReturnsService.GetBatchNO(obj.SKUCode).then(function (success) {
            $scope.PendingRequestDetails.RequestDetail[index].BatchList = success.data;
            $scope.PendingRequestDetails.RequestDetail[index].PackSize = null;
            $scope.PendingRequestDetails.RequestDetail[index].selectedSKU.Batch_No = null;
            $scope.PendingRequestDetails.RequestDetail[index].selectedSKU.Manufacturing_Date = null;
            $scope.PendingRequestDetails.RequestDetail[index].selectedSKU.Shelf_Life = null;
            $scope.PendingRequestDetails.RequestDetail[index].InvoiceQuantity = null;
            $scope.PendingRequestDetails.RequestDetail[index].SRVQuantity = null;
            //$scope.Request.RequestDetail[index].selectedSKU.Unit = null;
            $scope.PendingRequestDetails.RequestDetail[index].Volume = null;
            $scope.PendingRequestDetails.RequestDetail[index].SRVValue = null;
            $scope.PendingRequestDetails.RequestDetail[index].selectedComplaint = null;
            $scope.PendingRequestDetails.RequestDetail[index].SubreasonId = null;
            $scope.PendingRequestDetails.RequestDetail[index].Remarks = null;
        });
        //});
    };

    $scope.checkQuntity = function () {

        angular.forEach($scope.PendingRequestDetails.RequestDetail, function (value, key) {

            if ((value.InvoiceQuantity / 1) < (value.SRVQuantity / 1)) {
                value.SRVQuantity = '';
                swal("Warning", "SRV Quantity can not be more than Invoice Quantity", "warning");
                return false;
            }
        });
    };

    $scope.changeData = function (batchNo) {
        console.log($scope.PendingRequestDetails.RequestDetail);
        angular.forEach($scope.PendingRequestDetails.RequestDetail, function (value, key) {

            angular.forEach(value.BatchList, function (value1, key1) {
                if (value1.Batch_No == batchNo) {

                    value.Manufacturing_Date = value1.Manufacturing_Date;
                    value.Shelf_Life = value1.Shelf_Life;
                    value.Unit = value1.Unit;
                    value.PackSize = value1.Pack_Size;

                }
            });
        });
        console.log($scope.PendingRequestDetails.RequestDetail);

    };

    $scope.Add = function () {
        console.log("inadd");
        $scope.PendingRequestDetails.RequestDetail.push({

            InvoiceNo: '',
            InvoiceDate: '',
            ProvideGST_Yes: '',
            ProvideGST_No: '',
            InvoiceUpload: '',

            InvoiceQuantity: '',
            SRVQuantity: 0,
            Unit: '',
            PackSize: 0,
            Volume: 0,
            SRVValue: '',
            selectedComplaint: '',
            Remarks: ''
        });
        console.log("inadd " + $scope.PendingRequestDetails);
    };

    $scope.RemoveRow = function (index, Obj) {

        $scope.PendingRequestDetails.RequestDetail.splice(index, 1);

    };

    $scope.SaveAsDraft = function () {
        console.log($scope.PendingRequestDetails);
        //$scope.Request.DepotId = $scope.Request.DepotObj.DepotId;
        //$scope.Request.DealerId = $scope.Request.DealerObj.DealerId;
        //$scope.Request.ReasonForReturn_Id = $scope.Request.ReasonForReturnObj.RequestType_Id;

        //if (!$scope.Request.DepotId) {

        //    swal("Please select Depot");
        //    return false;
        //}
        //else if (!$scope.Request.DealerId) {

        //    swal("Please select Dealer");
        //    return false;
        //}
        //else if (!$scope.Request.ReasonForReturn_Id) {

        //    swal("Please select Reason for return   ");
        //    return false;
        //}

        //console.log($scope.Request);
        var fd = new FormData();
        var i = 0;
        angular.forEach($scope.PendingRequestDetails.RequestDetail, function (value, key) {

            if (value.ProvideGST_Yes) {
                if (value.UploadedInvoice && value.InvoiceUpload) {
                    value.UploadedInvoice = null;
                    console.log("for null");
                }
                fd.append(i + '_1', value.InvoiceUpload);
                i++;
            }
        });
        console.log(i);

        $scope.PendingRequestDetails.EmployeeCode = $rootScope.session.EMP_CODE;
        $scope.PendingRequestDetails.Country = $rootScope.session.Country;
        fd.append('data', angular.toJson($scope.PendingRequestDetails));

        swal({
            title: "Are you sure?",
            text: "You are going to save request!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#0373fc", confirmButtonText: "Yes",
            cancelButtonText: "No",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    SalesReturnsService.SaveAsDraft(fd).then(function (success) {
                        console.log(success.data);
                        if (success.data != 0) {
                            swal("Success", "Request successfully saved as draft.", "success");
                            $scope.PendingRequestDetails.RequestHeader_Id = success.data;
                        }
                        else {
                            swal("Error", success.data, "error");
                        }
                    }, function (error) {
                        console.log(error);
                    });
                } else {
                    //swal("Cancelled", "Your imaginary file is safe :)", "error");
                }
            });
    };

    $scope.CompareDate = function (invoiceDate) {

        if (invoiceDate) {
            var d1 = new Date($scope.LastDate);
            var d2 = new Date(invoiceDate);

            if (d2 > d1) {
                return false;
            }
            else {
                return true;
            }
        }
    };

    $scope.RecommendRequest = function (PendingRequestDetails) {
        var RequestType_Id = PendingRequestDetails.ReasonForReturn_Id;
        if ($scope.Remarks == "" || $scope.Remarks == null) {
            swal("Warning", "Please Enter Remarks", "warning");
            return false;
        }
        else {
            console.log($scope.CurrentStatus_Id);
            console.log($scope.Request_Id);
            console.log(RequestType_Id);

            var CurrentStatus_Id = 0;
            var FutureStatus_Id = 0;
            var Active_Role = 0;
            var Requested_Role = 0;

            if ($scope.CurrentStatus_Id == 1) {
                if (RequestType_Id == 1) {
                    CurrentStatus_Id = 10;//Recommended Complaint Handler
                    FutureStatus_Id = 3;//Pending Complaint Manager
                    Active_Role = 2;
                    Requested_Role = 3;
                }
                else if (RequestType_Id == 2) {

                    CurrentStatus_Id = 11; //Recommended Logistic Manager
                    FutureStatus_Id = 5; //Pending Logistic Head
                    Active_Role = 4;
                    Requested_Role = 5;
                }
                else if (RequestType_Id == 3) {
                    if (PendingRequestDetails.RequestTypeOption != 'D') {
                        CurrentStatus_Id = 14; //Recommended Regional Head
                        FutureStatus_Id = 26;//Sales Director in case of Depot person raise the request
                        Active_Role = 7;
                        Requested_Role = 9;//Sales Director in case of Depot person raise the request
                    }
                    else {
                        CurrentStatus_Id = 14; //Recommended Regional Head
                        FutureStatus_Id = 0;//setting future status on c#
                        Active_Role = 7;
                        Requested_Role = 0;//setting future status on c#
                    }
                }
            }
            else if ($scope.CurrentStatus_Id == 10 || $scope.CurrentStatus_Id == 11) {
                if (RequestType_Id == 1) {

                    CurrentStatus_Id = 12;//Approved Complaint Manager
                    FutureStatus_Id = 9;//Pending President
                    Active_Role = 3;
                    Requested_Role = 6;
                }
                else if (RequestType_Id == 2) {

                    CurrentStatus_Id = 13; //Approved Logistic Head
                    FutureStatus_Id = 9; //Pending President
                    Active_Role = 5;
                    Requested_Role = 6;
                }
            }

            swal({
                title: "Are you sure?",
                text: "You want to recommend this request?",
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
                        RequestType_Id: RequestType_Id,

                        Active_Role: Active_Role,
                        Requested_Role: Requested_Role,
                        Remarks: $scope.Remarks
                    };

                    PendingRequestService.RecommendRequest(Obj).then(function (success) {
                        $scope.init();
                        if (success.data.indexOf('Error') != -1) { swal("error", success.data, "error"); }
                        else { swal("Success", success.data, "success"); }
                    }, function (error) {
                        console.log(error);
                    });
                });
        }
    };

    $scope.RejectRequest = function (pt)//RequestType_Id,)
    {
        var RequestType_Id = pt.ReasonForReturn_Id;

        if ($scope.Remarks == "" || $scope.Remarks == null) {
            swal("Warning", "Please Enter Remarks", "warning");
            return false;
        }
        else {
            console.log($scope.CurrentStatus_Id);

            var CurrentStatus_Id = 15;
            var FutureStatus_Id = 0;
            var Active_Role = 0;
            var Requested_Role = 0;

            if ($scope.CurrentStatus_Id == 1) {
                if (RequestType_Id == 1) {
                    Active_Role = 2;
                }
                else if (RequestType_Id == 2) {
                    Active_Role = 4;
                }
                else if (RequestType_Id == 3) {

                    if ($scope.FutureStatus_Id == 9) {

                        Active_Role = 6;

                    } else if ($scope.FutureStatus_Id == 8) {

                        Active_Role = 9;
                    }
                    else if ($scope.FutureStatus_Id == 7) {

                        Active_Role = 8;

                    } else {

                        Active_Role = 7;
                    }
                }
            }
            else if ($scope.CurrentStatus_Id == 10 || $scope.CurrentStatus_Id == 11) {
                if (RequestType_Id == 1) {
                    if ($scope.FutureStatus_Id == 9) {

                        Active_Role = 6;
                    } else {
                        Active_Role = 3;
                    }
                }
                else if (RequestType_Id == 2) {
                    if ($scope.FutureStatus_Id == 9) {

                        Active_Role = 6;
                    } else {
                        Active_Role = 5;
                    }
                }
            }
            else if ($scope.CurrentStatus_Id == 12 || $scope.CurrentStatus_Id == 13 || $scope.CurrentStatus_Id == 14) {

                if (RequestType_Id == 1 || RequestType_Id == 2) {
                    if ($scope.FutureStatus_Id == 9) {

                        Active_Role = 6;
                    } else {
                        Active_Role = 6;
                    }
                }
                else if (RequestType_Id == 3) {
                    if ($scope.FutureStatus_Id == 7) {
                        if ($scope.FutureStatus_Id == 9) {

                            Active_Role = 6;
                        } else {

                            Active_Role = 8;
                        }
                    }
                    else if ($scope.FutureStatus_Id == 8) {
                        if ($scope.FutureStatus_Id == 9) {
                            Active_Role = 6;
                        }
                        else {
                            Active_Role = 9;
                        }
                    }
                    else {
                        Active_Role = 6;
                    }
                }
            }
            if (pt.RequestTypeOption == "D")
            {
                FutureStatus_Id = 10026;
                Requested_Role =7;
            }
            swal({
                title: "Are you sure?",
                text: "You want to reject this request?",
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
                        RequestType_Id: RequestType_Id,

                        Active_Role: Active_Role,
                        Requested_Role: Requested_Role,
                        Remarks: $scope.Remarks

                    };
                    console.log(Obj);

                    PendingRequestService.RejectRequest(Obj).then(function (success) {
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

    $scope.ReconsiderRequest = function (RequestType_Id) {

        if ($scope.Remarks == "" || $scope.Remarks == null) {
            swal("Warning", "Please Enter Remarks", "warning");
            return false;
        } else {

            var CurrentStatus_Id = 0; //Reconsider User
            var FutureStatus_Id = 17; //Pending User
            var Active_Role = 0;
            var Requested_Role = 1;

            if ($scope.CurrentStatus_Id == 1) {
                if (RequestType_Id == 1) {
                    if ($scope.FutureStatus_Id == 3) {

                        Active_Role = 3;
                        CurrentStatus_Id = 20;
                    }
                    else if ($scope.FutureStatus_Id == 9) {

                        Active_Role = 6;
                        CurrentStatus_Id = 23;
                    }
                    else {
                        Active_Role = 2;
                        CurrentStatus_Id = 18;
                    }
                }
                else if (RequestType_Id == 2) {
                    if ($scope.FutureStatus_Id == 5) {

                        Active_Role = 5;
                        CurrentStatus_Id = 21;
                    }
                    else if ($scope.FutureStatus_Id == 9) {

                        Active_Role = 6;
                        CurrentStatus_Id = 23;
                    }
                    else {
                        Active_Role = 4;
                        CurrentStatus_Id = 19;
                    }
                }
                else if (RequestType_Id == 3) {

                    if ($scope.FutureStatus_Id == 7) {

                        Active_Role = 8;
                        CurrentStatus_Id = 25;
                    }
                    else if ($scope.FutureStatus_Id == 8) {

                        Active_Role = 9;
                        CurrentStatus_Id = 26;
                    }
                    else if ($scope.FutureStatus_Id == 9) {

                        Active_Role = 6;
                        CurrentStatus_Id = 23;
                    }
                    else {
                        Active_Role = 7;
                        CurrentStatus_Id = 22;
                    }
                }
            }
            else if ($scope.CurrentStatus_Id == 10 || $scope.CurrentStatus_Id == 11) {

                if (RequestType_Id == 1) {

                    if ($scope.FutureStatus_Id == 3) {

                        Active_Role = 3;
                        CurrentStatus_Id = 20;
                    }

                    else if ($scope.FutureStatus_Id == 9) {
                        Active_Role = 6;
                        CurrentStatus_Id = 23;
                    }
                }
                else if (RequestType_Id == 2) {

                    if ($scope.FutureStatus_Id == 5) {

                        Active_Role = 5;
                        CurrentStatus_Id = 21;
                    }
                    else if ($scope.FutureStatus_Id == 9) {

                        Active_Role = 6;
                        CurrentStatus_Id = 23;
                    }


                }

            }
            else if ($scope.CurrentStatus_Id == 12 || $scope.CurrentStatus_Id == 13 || $scope.CurrentStatus_Id == 14) {
                // else if ($scope.CurrentStatus_Id == 16) {
                if (RequestType_Id == 1 || RequestType_Id == 2) {

                    Active_Role = 6;
                    CurrentStatus_Id = 23;
                }
                else if (RequestType_Id == 3) {
                    if ($scope.FutureStatus_Id == 7) {

                        Active_Role = 8;
                        CurrentStatus_Id = 25;
                    }
                    else if ($scope.FutureStatus_Id == 8) {

                        Active_Role = 9;
                        CurrentStatus_Id = 26;
                    }
                    else {
                        Active_Role = 6;
                        CurrentStatus_Id = 23;
                    }
                }
            }

            var Obj = {

                EmployeeCode: $rootScope.session.EMP_CODE,
                Country: $rootScope.session.Country,

                CurrentStatus_Id: CurrentStatus_Id,
                FutureStatus_Id: FutureStatus_Id,

                Request_Id: $scope.Request_Id,
                RequestType_Id: RequestType_Id,

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
                        PendingRequestService.ReconsiderRequest(Obj).then(function (success) {
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

    $scope.ApproveRequest = function (RequestType_Id) {

        if ($scope.Remarks == "" || $scope.Remarks == null) {
            swal("Warning", "Please Enter Remarks", "warning");
            return false;
        } else {
            var Active_Role = 0;

            if (RequestType_Id == 1 || RequestType_Id == 2) {
                if ($scope.FutureStatus_Id == 9) {

                    Active_Role = 6;
                }
                else if ($scope.FutureStatus_Id == 3) {

                    Active_Role = 3;
                }
                else if ($scope.FutureStatus_Id == 5) {

                    Active_Role = 5;
                }
            }
            else if (RequestType_Id == 3) {

                if ($scope.FutureStatus_Id == 6) {

                    Active_Role = 7;
                }
                else if ($scope.FutureStatus_Id == 7) {

                    Active_Role = 8;
                } else if ($scope.FutureStatus_Id == 8) {

                    Active_Role = 9;

                } else if ($scope.FutureStatus_Id == 9) {

                    Active_Role = 6;
                }
            }

            swal({
                title: "Are you sure?",
                text: "You want to approve this request?",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes",
                closeOnConfirm: false
            },
                function () {
                    var Obj =
                    {
                        EmployeeCode: $rootScope.session.EMP_CODE,
                        Country: $rootScope.session.Country,

                        CurrentStatus_Id: 16,
                        FutureStatus_Id: 10019,

                        Request_Id: $scope.Request_Id,
                        RequestType_Id: RequestType_Id,

                        Active_Role: Active_Role,
                        Requested_Role: 10,
                        Remarks: $scope.Remarks
                    };
                    console.log(Obj);
                    PendingRequestService.ApproveRequest(Obj).then(function (success) {
                        $scope.init();
                        swal("Success", success.data, "success");
                    }, function (error) {
                        console.log(error);
                    });
                });
        }
    };

    $scope.CloseRequest = function (PendingRequestDetails) {
        console.log(PendingRequestDetails);
        var RequestType_Id = PendingRequestDetails.ReasonForReturn_Id;
        if (PendingRequestDetails.IsCommercialSettlement === true && $scope.FutureStatus_Id == 10026) {
            if (!PendingRequestDetails.EPNo) {
                swal("Warning", "Please Provide EP Number", "warning");
                return false;
            }
            if (!PendingRequestDetails.ReasonForCommercialSettlement) {
                swal("Warning", "Please Specify Reason For Commercial Settlement", "warning");
                return false;
            }
        }
        else if (!PendingRequestDetails.DetailsForMaterialGoToDealer && $scope.FutureStatus_Id == 10026) {
            swal("Warning", "Please Provide Details For Material!", "warning");
            return false;
        }
        else if ($scope.FutureStatus_Id == 10027 && !PendingRequestDetails.DocketNumber) {
            swal("Warning", "Please Enter Docket Number!", "warning");
            return false;
        }
        if ($scope.Remarks == "" || $scope.Remarks == null) {
            swal("Warning", "Please Enter Remarks", "warning");
            return false;
        }
        else
        {
            var CurrentStatus_Id = 10029;
            var FutureStatus_Id = 0;
            var Active_Role = 7;
            var Requested_Role = 0;

            if (PendingRequestDetails.IsCommercialSettlement === true && $scope.FutureStatus_Id == 10026) {
                CurrentStatus_Id = 10029;
                FutureStatus_Id = 0;
                Active_Role = 7;
                Requested_Role = 0;
            }
            else if (PendingRequestDetails.IsCommercialSettlement === false && $scope.FutureStatus_Id == 10026) {
                CurrentStatus_Id = 10028;
                FutureStatus_Id = 10027;
                Active_Role = 7;
                Requested_Role = 10;
            }
            else if ($scope.FutureStatus_Id == 10027) {
                CurrentStatus_Id = 10030;
                FutureStatus_Id = 0;
                Active_Role = 10;
                Requested_Role = 0;
            }

            swal({
                title: "Are you sure?",
                text: "You are going to submit!",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes",
                closeOnConfirm: false
            },
                function () {
                    var Obj = {
                        Request: PendingRequestDetails,
                        EmployeeCode: $rootScope.session.EMP_CODE,
                        Country: $rootScope.session.Country,

                        CurrentStatus_Id: CurrentStatus_Id,
                        FutureStatus_Id: FutureStatus_Id,

                        Request_Id: $scope.Request_Id,
                        RequestType_Id: RequestType_Id,

                        Active_Role: Active_Role,
                        Requested_Role: Requested_Role,
                        Remarks: $scope.Remarks
                    };

                    PendingRequestService.CloseRequest(Obj).then(function (success) {
                        if (success.data.indexOf('Error') != -1) { swal("error", success.data, "error"); }
                        else { $scope.init(); swal("Success", success.data, "success"); }
                    }, function (error) {
                        console.log(error);
                    });
                });
        }
    };

    $scope.init();
});

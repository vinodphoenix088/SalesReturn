var app = angular.module('InprocessRequestModule', []);

app.service('InprocessRequestService', function ($http, $location, HomeService) {




    this.getInprocessRequest = function (EmployeeCode) {

        return $http.get(HomeService.AppUrl + 'Inprocess/getInprocessRequest?EmployeeCode=' + EmployeeCode, {});

    };

});



app.controller('InprocessRequestController', function ($scope, HomeService, RequestTypeService, $uibModal, PendingRequestService, InprocessRequestService, $rootScope, SalesReturnsService, $filter) {
    console.log("InprocessRequest");

 
    $scope.init = function () {
        $scope.IsReconsidered = false;


        $scope.TodayDate = new Date();

        $scope.InprocessRequestList = [];

        console.log($rootScope.session);

        $scope.InprocessRequestGrid = true;
        $scope.InprocessRequestForm = false;




        InprocessRequestService.getInprocessRequest($rootScope.session.EMP_CODE).then(function (success) {

            $scope.InprocessRequestList = success.data;


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

        ///Code for excess and Short
        $scope.ReadRecvdQuantity = function () {

            angular.forEach($scope.InprocessRequestService.RequestDetail, function (value, key) {
                value.Short = value.SRVQuantity - value.ReceivedQuantity;
                value.Excess = value.ReceivedQuantity - value.SRVQuantity;
            });

        };



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
                $scope.InprocessRequestDetails.DealerObj = $scope.DealerList.find(x=>x.DealerId == $scope.InprocessRequestDetails.DealerId);

            }

        }, function (error) {

            console.log(error);

        });

    };
    $scope.ViewRequestDetails = function (Id, CurrentStatus_Id, FutureStatus_Id, CreatedBy , Obj) {

        $scope.CurrentStatus_Id = CurrentStatus_Id;
        $scope.Request_Id = Id;
        $scope.FutureStatus_Id = FutureStatus_Id;


        $scope.TotalSRV_ = Obj.TotalSRV;
        $scope.CreatedDate_ = Obj.CreatedDate;
        $scope.CreatedBy_ = Obj.CreatedBy + ":" + Obj.CreatedBy_EMP_CODE;

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


            $scope.InprocessRequestGrid = false;
            $scope.InprocessRequestForm = true;

            $scope.InprocessRequestDetails = success.data;
            console.log(success.data);

            angular.forEach($scope.InprocessRequestDetails.RequestDetail, function (value, key) {

                SalesReturnsService.GetBatchNO(value.SKUCode).then(function (success) {

                    value.BatchList = success.data;

                });

            });

          

            SalesReturnsService.getSubReasonList($scope.InprocessRequestDetails.ReasonForReturn_Id).then(function (success) {

                $scope.SubReasonList = success.data;

            }, function (error) {
                console.log(error);
            });


            if (FutureStatus_Id == 17 && CreatedBy == $rootScope.session.EMP_CODE) {



                $scope.IsReconsidered = true;
                $scope.GetDealerList($scope.InprocessRequestDetails.DepotId);

                angular.forEach($scope.InprocessRequestDetails.RequestDetail, function (value, key) {

                    value.InvoiceDate = $filter('date')(value.InvoiceDate, "dd MMM yyyy");
                });
                
                $scope.InprocessRequestDetails.DepotObj = $scope.DepotList.find(x=>x.DepotId == $scope.InprocessRequestDetails.DepotId);
               
                $scope.InprocessRequestDetails.ReasonForReturnObj = $scope.RequestTypeList.find(x=>x.RequestType_Id == $scope.InprocessRequestDetails.ReasonForReturn_Id);

                console.log($scope.InprocessRequestDetails);
            }


        }, function (error) {
            console.log(error);
        });


    };



    $scope.UpdateRequest = function () {

        console.log($scope.InprocessRequestDetails);

        $scope.InprocessRequestDetails.DepotId = $scope.InprocessRequestDetails.DepotObj.DepotId;
        $scope.InprocessRequestDetails.DealerId = $scope.InprocessRequestDetails.DealerObj.DealerId;
        $scope.InprocessRequestDetails.ReasonForReturn_Id = $scope.InprocessRequestDetails.ReasonForReturnObj.RequestType_Id;


        var check = true;
        if (!$scope.InprocessRequestDetails.DepotId) {

            swal("Please select Depot");
            return false;
        }
        else if (!$scope.InprocessRequestDetails.DealerId) {

            swal("Please select Dealer");
            return false;
        }
        else if (!$scope.InprocessRequestDetails.ReasonForReturn) {

            swal("Please select Reason for return   ");
            return false;
        }

        angular.forEach($scope.InprocessRequestDetails.RequestDetail, function (value, key) {

            var d1 = new Date('2019-07-30');
            var d2 = new Date(value.InvoiceDate);
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
            else if ($scope.InprocessRequestDetails.ReasonForReturn_Id == 1 && !value.selectedComplaint) {

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
            console.log($scope.InprocessRequestDetails);
            var fd = new FormData();
            var i = 0;
            angular.forEach($scope.InprocessRequestDetails.RequestDetail, function (value, key) {

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


            $scope.InprocessRequestDetails.EmployeeCode = $rootScope.session.EMP_CODE;
            $scope.InprocessRequestDetails.Country = $rootScope.session.Country;

            console.log($scope.InprocessRequestDetails);
            fd.append('data', angular.toJson($scope.InprocessRequestDetails));

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
        }


    };

    $scope.GetBatchNO = function (obj) {

        console.log(obj);

        //SalesReturnsService.GetBatchNO(obj.SKUCode).then(function (success) {

        //    $scope.BatchList = success.data;

            angular.forEach($scope.InprocessRequestDetails.RequestDetail, function (value, key) {

                SalesReturnsService.GetBatchNO(value.selectedSKU.SKUCode).then(function (success) {

                    value.BatchList = success.data;

                });

            });

           
        //});

    };

    $scope.checkQuntity = function () {

        angular.forEach($scope.InprocessRequestDetails.RequestDetail, function (value, key) {

            if ((value.InvoiceQuantity / 1) < (value.SRVQuantity / 1)) {
                value.SRVQuantity = '';
                swal("Warning", "SRV Quantity can not be more than Invoice Quantity", "warning");
                return false;
            }



        });


    };
    $scope.changeData = function (batchNo) {
        console.log($scope.InprocessRequestDetails.RequestDetail);
        angular.forEach($scope.InprocessRequestDetails.RequestDetail, function (value, key) {

            angular.forEach(value.BatchList, function (value1, key1) {
                if (value1.Batch_No == batchNo) {

                    value.Manufacturing_Date = value1.Manufacturing_Date;
                    value.Shelf_Life = value1.Shelf_Life;
                    value.Unit = value1.Unit;
                    value.PackSize = value1.Pack_Size;

                }
            });
        });
        console.log($scope.InprocessRequestDetails.RequestDetail);

    };

    $scope.Add = function () {

        console.log("inadd");
        $scope.InprocessRequestDetails.RequestDetail.push({

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

        console.log("inadd " + $scope.InprocessRequestDetails);
    };



    $scope.RemoveRow = function (index, Obj) {

        $scope.InprocessRequestDetails.RequestDetail.splice(index, 1);

    };

    $scope.SaveAsDraft = function () {
        console.log($scope.InprocessRequestDetails);
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
        angular.forEach($scope.InprocessRequestDetails.RequestDetail, function (value, key) {

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


        $scope.InprocessRequestDetails.EmployeeCode = $rootScope.session.EMP_CODE;
        $scope.InprocessRequestDetails.Country = $rootScope.session.Country;
        fd.append('data', angular.toJson($scope.InprocessRequestDetails));


        SalesReturnsService.SaveAsDraft(fd).then(function (success) {
            console.log(success.data);

            if (success.data != 0) {
                swal("Success", "Request successfully saved as draft.", "success");

                
                $scope.InprocessRequestDetails.RequestHeader_Id = success.data;
            }
            else {

                swal("Error", success.data, "error");
            }
        }, function (error) {
            console.log(error);

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

    $scope.init();
});

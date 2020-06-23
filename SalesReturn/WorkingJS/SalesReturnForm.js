//import { console } from "../bower_components/inputmask/dist/inputmask/global/window";

//import { console } from "../bower_components/inputmask/dist/inputmask/global/window";

//import { console } from "../bower_components/inputmask/dist/inputmask/global/window";

var app = angular.module('SalesReturnModule', []);

app.service('SalesReturnsService', function ($http, $location) {

    this.getDummyRequestDetailObj = function () {
        return $http.get('api/Request/getDummyRequestDetailObj', {});
    };
    this.GetDepotList = function (data) {
        return $http.get('api/Request/GetDepotListBasedOnEmpCode?EmpCode='+data, {});
    };

    this.GetDealerList = function (Depot_Id) {
        return $http.get('api/Request/GetDealerList?Depot_Id=' + Depot_Id, {});
    };

    this.GetSKUCode = function () {
        return $http.get('api/Request/GetSKUCode', {});
    };

    this.GetCustomerComplaintList = function () {
        return $http.get('api/Request/GetCustomerComplaintList', {});
    };

    this.saveRequest = function (fd) {
        return $http.post("/api/Request/saveRequest/", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };

    this.SaveAsDraft = function (fd) {
        return $http.post("/api/Request/SaveAsDraft/", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };

    this.GetSavedAsDraftRequest = function (EmpCode, Depot_Id, Dealer_Id, Reason_Id) {
        return $http.get('api/Request/GetSavedAsDraftRequest?EmpCode=' + EmpCode + '&Depot_Id=' + Depot_Id + '&Dealer_Id=' + Dealer_Id + '&Reason_Id=' + Reason_Id, {});
    };

    this.getSubReasonList = function (ReasonId) {
        return $http.get('api/RequestType/GetSubReasonList?Id=' + ReasonId, {});
    };

    this.GetBatchNO = function (SkuCode) {
        return $http.get('api/RequestType/GetBatchNO?SkuCode=' + SkuCode, {});
    };
    this.GetLastThreeMonthDate = function () {
        return $http.get('api/RequestType/GetLastThreeMonthDate', {});
    };

    this.GetInvoiceDataRequest = function (obj) {
        return $http.post('api/RequestType/GetInvoiceDataRequest', obj, {});
    };
});

app.controller('SalesReturnController', function ($scope, HomeService, SalesReturnsService, RequestTypeService, $rootScope, $window, $filter,$q) {
    
    $scope.TodayDate = new Date();

    if ($rootScope.session.IsDepotPerson == true) {
    $scope.Request = {
        //DepotObj: {
            //DepotCode: $rootScope.session.DepotCode,
            //DepotName: $rootScope.session.DepotName,
            //DepotId: $rootScope.session.DepotId
        //}
    };
        console.log($scope.Request);
    }

    $scope.peopleObj = [
        { name: 'Adam', email: 'adam@email.com', age: 12, country: 'United States' },
        { name: 'Amalie', email: 'amalie@email.com', age: 12, country: 'Argentina' },
        { name: 'Estefanía', email: 'estefania@email.com', age: 21, country: 'Argentina' },
        { name: 'Adrian', email: 'adrian@email.com', age: 21, country: 'Ecuador' },
        { name: 'Wladimir', email: 'wladimir@email.com', age: 30, country: 'Ecuador' },
        { name: 'Samantha', email: 'samantha@email.com', age: 30, country: 'United States' },
        { name: 'Nicole', email: 'nicole@email.com', age: 43, country: 'Colombia' },
        { name: 'Natasha', email: 'natasha@email.com', age: 54, country: 'Ecuador' },
        { name: 'Michael', email: 'michael@email.com', age: 15, country: 'Colombia' },
        { name: 'Nicolás', email: 'nicole@email.com', age: 43, country: 'Colombia' }
    ];

    $scope.init = function () {

        SalesReturnsService.GetSKUCode().then(function (success) {
            $scope.SKUCodeList = success.data;
            //if ($rootScope.session.IsDepotPerson == true) {
                $scope.Request = {
                    RequestDetail: [{
                        InvoiceQuantity: 0,
                        PackSize: null
                    }]//,
                    //DepotObj: {
                        //DepotCode: $rootScope.session.DepotCode,
                        //DepotName: $rootScope.session.DepotName,
                        //DepotId: $rootScope.session.DepotId
                    //}
                };
               // $scope.GetDealerList($scope.Request.DepotObj);
            //}
            //else {
            //$scope.Request = {
            //    RequestDetail: [{
            //        InvoiceQuantity: 0,
            //        PackSize: null
            //    }], 
            //}; }
            console.log($scope.Request);
            return SalesReturnsService.GetSKUCode();
        }).then(function (success) {
            $scope.SKUCodeList = success.data;
            return SalesReturnsService.GetDepotList($rootScope.session.EMP_CODE);
           // return SalesReturnsService.GetDepotList('0');
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
            //console.log(error);

        });
    };

    $scope.ResetProvideGST = function (index) {
        //console.log(index);
        $scope.Request.RequestDetail[index].ProvideGST_No = false;
        $scope.Request.RequestDetail[index].ProvideGST_Yes = false;
    };

    $scope.GetBatchNO = function (obj,index) {
        console.log(index);
        console.log(angular.isUndefined(index));
        //angular.forEach($scope.Request.RequestDetail, function (value, key) {
        SalesReturnsService.GetBatchNO(obj.SKUCode).then(function (success) {
            debugger;
                $scope.Request.RequestDetail[index].BatchList = success.data;
                $scope.Request.RequestDetail[index].PackSize = null;
                $scope.Request.RequestDetail[index].selectedSKU.Batch_No = null;
                $scope.Request.RequestDetail[index].selectedSKU.Manufacturing_Date=null;
                $scope.Request.RequestDetail[index].selectedSKU.Shelf_Life =null;
                $scope.Request.RequestDetail[index].InvoiceQuantity = null;
                $scope.Request.RequestDetail[index].SRVQuantity = null;
                //$scope.Request.RequestDetail[index].selectedSKU.Unit = null;
                $scope.Request.RequestDetail[index].Volume = null;
                $scope.Request.RequestDetail[index].SRVValue = null;
                $scope.Request.RequestDetail[index].selectedComplaint = null;
                $scope.Request.RequestDetail[index].SubreasonId = null;
                $scope.Request.RequestDetail[index].Remarks = null;
            });
        //});
    };

    $scope.checkQuntity = function () {

        angular.forEach($scope.Request.RequestDetail, function (value, key) {
            if ((value.InvoiceQuantity / 1) < (value.SRVQuantity / 1)) {
                value.SRVQuantity = '';
                swal("Warning", "SRV Quantity can not be more than Invoice Quantity", "warning");
                return false;
            }
        });
    };

    $scope.changeData = function (batchNo,index) {
        //angular.forEach($scope.Request.RequestDetail[index], function (value, key) {
        angular.forEach($scope.Request.RequestDetail[index].BatchList, function (value1, key1) {
            if (value1.Batch_No == batchNo) {
                $scope.Request.RequestDetail[index].selectedSKU.Manufacturing_Date = value1.Manufacturing_Date;
                $scope.Request.RequestDetail[index].selectedSKU.Shelf_Life = value1.Shelf_Life;
            }
        });
        //});
    };

    $scope.GetDealerList = function (DepotObj) {

        //console.log(DepotObj);
        SalesReturnsService.GetDealerList(DepotObj.DepotId).then(function (success) {

            if (success.data != null) {
                $scope.DealerList = success.data;
                $scope.Request.DealerObj = {};
            }

        }, function (error) {
            //console.log(error);
        });
    };

    $scope.CopyRequestDetail = [];
    $scope.SelectSKU = function (event) {

        //console.log(event);
    };

    $scope.Update = function () {
        $scope.check = true;
        $scope.apicheck = false;
        console.log("Before goming isside $API_Check");

        $scope.$API_Check().then(function (Obj) {
            var fd = new FormData();
            var i = 0;
            var remarks = "";
            angular.forEach($scope.Request.RequestDetail, function (value, key) {
                debugger;

                if (value.ProvideGST_Yes) {
                    fd.append(i + '_1', value.InvoiceUpload);
                    i++;
                }
                Remarks = "1. " + value.Remarks + "/n";
            });

            var k = true;

            angular.forEach($scope.Request.RequestDetail, function (value, key) {


                angular.forEach($scope.Request.RequestDetail, function (value1, key1) {

                    if (key!=key1 && value1.InvoiceNo == value.InvoiceNo && value1.selectedSKU.SKUCode == value.selectedSKU.SKUCode && value1.selectedSKU.Batch_No == value.selectedSKU.Batch_No) {

                        k = false;
                    };
                });

                //$scope.CopyRequestDetail.push(value);
            });


            if (k == false) {
                swal("error", "The combination of Invoice no, SKU Name and Batch no should not be same.", "error");
                return false;
            }
            else {
                
                for (var i = 0; i < $scope.Request.RequestDetail.length; i++) {
                    if ($scope.Request.RequestDetail[i].selectedSKU.Batch_No == 'Other' && ($scope.Request.RequestDetail[i].selectedSKU.BatchNoText == null || $scope.Request.RequestDetail[i].selectedSKU.BatchNoText == '')) {
                        swal("error", "Please Enter Batch Number", "error");
                        return false;
                    }
                }
                for (var i = 0; i < $scope.Request.RequestDetail.length; i++) {
                    if ($scope.Request.RequestDetail[i].selectedSKU.Batch_No == 'Other' && ($scope.Request.RequestDetail[i].selectedSKU.MGfDate == null || $scope.Request.RequestDetail[i].selectedSKU.MGfDate == '')) {
                        swal("error", "Please Enter manufacturing Date", "error");
                        return false;
                    }
                }

                for (var i = 0; i < $scope.Request.RequestDetail.length; i++) {
                    if ($scope.Request.RequestDetail[i].selectedSKU.Batch_No == 'Other' && ($scope.Request.RequestDetail[i].selectedSKU.MGfDate.getTime() > Date.parse($scope.Request.RequestDetail[i].InvoiceDate))){
                        swal("error", "Manufacturing Date can not be greater than Invoice Date", "error");
                        return false;
                    }
                }
                $scope.Request.Remarks = Remarks;
                $scope.Request.EmployeeCode = $rootScope.session.EMP_CODE;
                $scope.Request.Country = $rootScope.session.Country;
                fd.append('data', angular.toJson($scope.Request));
                swal({
                    title: "Are you sure?",
                    text: "You are going to submit request!",
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
                                if (success.data.indexOf("Success") != -1) {
                                    swal("Success", success.data, "success");
                                    $window.location.href = '#/InprocessRequest';
                                }
                                else {
                                    swal("Error", success.data, "error");
                                }
                            });
                        }

                    });
            }
        }, function (reject) {
            swal("", reject, "error");
            console.log(reject);
           });
    };

    $scope.$API_Check = function () {
        var deferred = $q.defer();

        $scope.Request.DepotId = $scope.Request.DepotObj.DepotId;
        $scope.Request.DealerId = $scope.Request.DealerObj.DealerId;
        $scope.Request.ReasonForReturn_Id = $scope.Request.ReasonForReturnObj.RequestType_Id;

        angular.forEach($scope.Request.RequestDetail, function (value, key) {
            if (new Date(value.InvoiceDate) < new Date(value.selectedSKU.Manufacturing_Date)) {
                deferred.reject('Invoice Date can not be less than SKU MFG Date!');
            }
            angular.forEach($scope.Request.RequestDetail, function (value1, key1) {
                if (value.key !=value1.key1 && value1.InvoiceNo == value.InvoiceNo && value1.selectedSKU.SKUCode == value.selectedSKU.SKUCode && value1.selectedSKU.Batch_No == value.selectedSKU.Batch_No) {
                    deferred.reject('Duplicate Invoice Details are not allowed!');
                    return deferred.promise;
                };
                if (value1.SRVQuantity <= 0) {
                    deferred.reject('SRV Quantity can not be less than or equal to zero!');
                    return deferred.promise;
                };
                if (value1.SRVValue<=0) {
                    deferred.reject('SRV Value can not be less than or equal to zero!');
                    return deferred.promise;
                };
                
            });
        });
        
        if (!$scope.Request.DepotId) {
            deferred.reject('Please select Depot');
            //swal("Please select Depot");
            //return false;
        }
        else if (!$scope.Request.DealerId) {
            deferred.reject('Please select Dealer');
            //swal("Please select Dealer");
            //return false;
        }
        else if (!$scope.Request.ReasonForReturn_Id) {
            deferred.reject('Please select Reason for return   ');
            //swal("Please select Reason for return   ");
            //return false;
        }

        angular.forEach($scope.Request.RequestDetail, function (value, key) {
            var d1 = new Date($scope.LastDate);
            var d2 = new Date(value.InvoiceDate);
            if (value.InvoiceNo && value.selectedSKU.SKUCode && value.selectedSKU.Batch_No) {
                var invObj = {
                    InvoiceNo: value.InvoiceNo,
                    SKUCode: value.selectedSKU.SKUCode,
                    BatchNo: value.selectedSKU.Batch_No
                };

                SalesReturnsService.GetInvoiceDataRequest(invObj).then(function (success) {
                    
                    $scope.CheckInvDetail = success.data;
                    // var quantityCheck = CheckInvDetail.InvoiceQuantity - CheckInvDetail.ReceivedQTY;
                    if ($scope.CheckInvDetail == null) {
                        deferred.resolve('true');
                        //$scope.check = true;
                    }
                    else
                    {
                        if ($scope.CheckInvDetail.InvoiceQuantity != value.InvoiceQuantity) {

                            //swal("Invoice Quantity should be same as previous generated invoice quantity of this invoice no. : " + $scope.CheckInvDetail.InvoiceNumber + "");
                            deferred.reject('Invoice Quantity should be same as previous generated invoice quantity of this invoice no. : ' + $scope.CheckInvDetail.InvoiceNumber + '');
                            //$scope.check = false;
                            //return false;
                        }

                        if ($scope.CheckInvDetail.InvoiceQuantity == value.InvoiceQuantity && value.SRVQuantity > $scope.CheckInvDetail.RemainingQTY) {

                            //swal("SRV Quantity can not be greater than Remaining Quantity : " + $scope.CheckInvDetail.RemainingQTY + "");
                            //$scope.check = false;
                            deferred.reject('SRV Quantity can not be greater than Remaining Quantity : ' + $scope.CheckInvDetail.RemainingQTY);
                            //return false;
                        }
                        if ($scope.CheckInvDetail.InvoiceQuantity == value.InvoiceQuantity && $scope.CheckInvDetail.Status_Id < 10023) {

                            //swal("Request can not be genrated because previous request is not approved by CSO ");
                            //$scope.check = false;
                            deferred.reject('Request can not be genrated because previous request is not approved by CSO ');
                            //return false;
                        }
                    }
                });
            }

            if (!value.InvoiceNo) {

                //swal("Please enter invoice number");
                //$scope.check = false;
                deferred.reject('Please enter invoice number');
                //return false;
            }
            else if (!value.InvoiceDate) {

                //swal("Please enter invoice date");
                deferred.reject('Please enter invoice date');
                //$scope.check = false;
                //return false;

            } else if (!value.ProvideGST_Yes && !value.ProvideGST_No && (d2 < d1)) {

                //swal("Please select checkbox for providing GST invoice");
                deferred.reject('Please select checkbox for providing GST invoice');
                //$scope.check = false;
                //return false;
            } else if (value.ProvideGST_Yes && !value.InvoiceUpload) {

                //swal("Please upload invoice copy");
                deferred.reject('Please upload invoice copy');
                //$scope.check = false;
                //return false;
            }

            else if (!value.selectedSKU) {

                //swal("Please enter SKU code");
                deferred.reject('Please enter SKU code');
                //$scope.check = false;
                //return false;

            } else if (!value.InvoiceQuantity) {

                //swal("Please enter invoice quantity");
                deferred.reject('Please enter invoice quantity');
                //$scope.check = false;
                //return false;

            } else if (!value.SRVQuantity) {

                //swal("Please enter SRV quantity");
                deferred.reject('Please enter SRV quantity');
                //$scope.check = false;
                //return false;

            }

            else if (!value.SRVValue) {

                //swal("Please enter SRV Value");
                deferred.reject('Please enter SRV Value');
                //$scope.check = false;
                //return false;
            }
            else if ($scope.Request.ReasonForReturn_Id == 1 && !value.selectedComplaint) {

                //swal("Please enter Complaint number");
                deferred.reject('Please enter Complaint number');
                //$scope.check = false;
                //return false;
            }
            else if (!value.SubreasonId) {

                //swal("Please select Subreason");
                deferred.reject('Please select Subreason');
                //$scope.check = false;
                //return false;
            }
            else if (!value.Remarks) {

                //swal("Please enter Remarks");
                deferred.reject('Please enter Remarks');
                //$scope.check = false;
                //return false;
            }
        });
        return deferred.promise;
    };

    $scope.Add = function () {
        //console.log("inadd");
        $scope.Request.RequestDetail.push({
            InvoiceNo: '',
            InvoiceDate: '',
            ProvideGST_Yes: '',
            ProvideGST_No: '',
            InvoiceUpload: '',
            // SKUCodeList : $scope.SKUCodeList,
            InvoiceQuantity: '',
            SRVQuantity: 0,
            Unit: '',
            PackSize: 0,
            Volume: 0,
            SRVValue: '',
            selectedComplaint: '',
            Remarks: '',
            BatchList:[]
        });
    };

    $scope.RemoveRow = function (index, Obj) {

        $scope.Request.RequestDetail.splice(index, 1);
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

    $scope.SaveAsDraft = function () {

        $scope.Request.DepotId = $scope.Request.DepotObj.DepotId;
        $scope.Request.DealerId = $scope.Request.DealerObj.DealerId;
        $scope.Request.ReasonForReturn_Id = $scope.Request.ReasonForReturnObj.RequestType_Id;

        if (!$scope.Request.DepotId) {

            swal("Please select Depot");
            return false;
        }
        else if (!$scope.Request.DealerId) {

            swal("Please select Dealer");
            return false;
        }
        else if (!$scope.Request.ReasonForReturn_Id) {

            swal("Please select Reason for return   ");
            return false;
        }

        //console.log($scope.Request);
        var fd = new FormData();
        var i = 0;
        angular.forEach($scope.Request.RequestDetail, function (value, key) {

            if (value.ProvideGST_Yes) {
                if (value.UploadedInvoice && value.InvoiceUpload) {
                    value.UploadedInvoice = null;
                    //console.log("for null");
                }
                fd.append(i + '_1', value.InvoiceUpload);
                i++;
            }
        });
        //console.log(i);

        $scope.Request.EmployeeCode = $rootScope.session.EMP_CODE;
        $scope.Request.Country = $rootScope.session.Country;
        fd.append('data', angular.toJson($scope.Request));

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
                        if (success.data != 0) {
                            swal("Success", "Request successfully saved as draft.", "success");
                            $window.location.href = '#/SavedRequest';
                            $scope.Request.RequestHeader_Id = success.data;
                        }
                        else {
                            swal("Error", success.data, "error");
                        }
                    }, function (error) {
                    });
                } else {
                }
            });
    };

    $scope.GetSavedAsDraftRequest = function () {

        console.log($scope.Request.ReasonForReturnObj);
        SalesReturnsService.getSubReasonList($scope.Request.ReasonForReturnObj.RequestType_Id).then(function (success) {

            $scope.SubReasonList = success.data;

        }, function (error) {
            console.log(error);
        });

        console.log($scope.Request);
        if ($scope.Request.DepotObj && $scope.Request.DealerObj && $scope.Request.ReasonForReturnObj) {

            SalesReturnsService.GetSavedAsDraftRequest($rootScope.session.EMP_CODE, $scope.Request.DepotObj.DepotId, $scope.Request.DealerObj.DealerId,
                $scope.Request.ReasonForReturnObj.RequestType_Id).then(function (success) {
                    console.log(success.data)
                    if (success.data != null) {
                        if (success.data.RequestDetail.length == 0) {
                            $scope.Request.RequestDetail = [{
                                InvoiceQuantity: 0,
                                PackSize: 0
                            }];
                            $scope.Request.RequestHeader_Id = success.data.RequestHeader_Id;
                        }
                        else {
                            $scope.Request.RequestDetail = success.data.RequestDetail;
                            $scope.Request.RequestHeader_Id = success.data.RequestHeader_Id;

                            angular.forEach($scope.Request.RequestDetail, function (value, key) {
                                value.InvoiceDate = $filter('date')(value.InvoiceDate, "dd MMM yyyy");
                                console.log(value);
                                if (value.SKUCode) {
                                    var SKUSval = $scope.SKUCodeList.find(o => o.SKUCode === value.SKUCode);
                                    console.log(SKUSval);
                                    if (value.SKUCode == SKUSval.SKUCode) {
                                        value.selectedSKU = SKUSval;
                                    }
                                }
                            });
                            console.log($scope.Request);
                        }
                    }
                    else {
                        $scope.Request.RequestDetail = [{
                            InvoiceQuantity: 0,
                            PackSize: 0
                        }];
                    }
                    console.log(success.data);
                }, function (error) {
                    console.log(error);
                });
        }
    };

    $scope.init();
});

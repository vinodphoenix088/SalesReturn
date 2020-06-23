

var app = angular.module('SRVCloserModule', []);

app.service('SRVCloserService', function ($http, $location, HomeService) {

    this.UpdateFlowList = function (Obj) {
        return $http.post(HomeService.AppUrl + 'Request/UpdateSRVCloserList', Obj, {});
    };

    this.getRequestforDepot = function (EmpCode) {
        return $http.get(HomeService.AppUrl + 'Request/getRequestforDepot?EmpCode=' + EmpCode , {});
    };
    
    this.GetSAPReasonDetailByReason = function (ReqType) {
        return $http.get(HomeService.AppUrl + 'RequestType/GetSAPReasonDetailByReason?ReqType=' + ReqType, {});
    };

    this.AcknowledgeRequestbyDepot = function (Obj) {
        return $http.post(HomeService.AppUrl + 'Request/AcknowledgeRequestbyDepot', Obj, {});
    };

    this.getSubReasonList = function (ReasonId) {
        return $http.get('api/RequestType/GetSubReasonList?Id=' + ReasonId, {});
    };
});



app.controller('SRVCloserController', function ($scope, HomeService, $uibModal, RequestTypeService, CSOReasonService, ApprovedRequestService, ReasonService, PendingRequestService, SRVCloserService, $rootScope, SalesReturnsService)
{
    $scope.init = function () {

        $scope.showView = false;

        SRVCloserService.getRequestforDepot($rootScope.session.EMP_CODE).then(function (success) {

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
        $scope.CreatedBy_ = Obj.CreatedBy + ":" +Obj.CreatedBy_EMP_CODE ;

        PendingRequestService.GetRequestDetails(Id, CurrentStatus_Id, FutureStatus_Id).then(function (success) {

            $scope.ApprovedRequestGrid = false;
            $scope.ApprovedRequestForm = true;

            $scope.ApprovedRequestDetails = success.data;
            console.log($scope.ApprovedRequestDetails);

            angular.forEach($scope.ApprovedRequestDetails.RequestDetail, function (value, key) {
                value.Damaged = 0;
            });

            return SRVCloserService.GetSAPReasonDetailByReason($scope.ApprovedRequestDetails.ReasonForReturn_Id);
        }).then(function (success) {

            $scope.SAPReasonList = success.data;

            return SRVCloserService.getSubReasonList($scope.ApprovedRequestDetails.ReasonForReturn_Id);
        }).then(function (success) {

            $scope.SubReasonList = success.data;

        }, function (error) {
            console.log(error);
        });
    };

    $scope.ReadRecvdQuantity = function () {

        angular.forEach($scope.ApprovedRequestDetails.RequestDetail, function (value, key) {

            value.Short = value.SRVQuantity - value.ReceivedQuantity;
            value.Excess = value.ReceivedQuantity - value.SRVQuantity;

        });

    };


    $scope.openStatus = function (id,reqoption) {
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


    $scope.AddNewRow = function () {

        $scope.AdminList.push({

            EmployeeCode: ""
        });

    };

    $scope.updateDetail = function (ReqDetail) {

        console.log($scope.ApprovedRequestDetails);
        var check = true;

        angular.forEach($scope.ApprovedRequestDetails.RequestDetail, function (value, key) {
            
            if (value.ReceivedQuantity == null || value.ReceivedQuantity=="") {
                swal("Please Enter the Receiving Quantity");
                check = false;
                return false;
            }
            //if (value.SAPsubReasonID == null || value.SAPsubReasonID == "") {
            //    swal("Please select the SAP Sub Reason");
            //    check = false;
            //    return false;
            //}
            if ((value.ReceivedQuantity/1) > (value.InvoiceQuantity/1)) {
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

            if ($scope.CurrentStatus_Id == 16)
            {
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

                    SRVCloserService.AcknowledgeRequestbyDepot(Obj).then(function (success) {
                        $scope.init();
                        swal("Success", success.data, "success");

                    }, function (error) {
                        console.log(error);
                    });
                });
        };
    };

    $scope.init();
});

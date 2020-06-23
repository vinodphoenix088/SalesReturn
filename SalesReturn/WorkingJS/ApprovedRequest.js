var app = angular.module('ApprovedRequestModule', []);

app.service('ApprovedRequestService', function ($http, $location, HomeService) {

    this.getApprovedRequest = function (EmployeeCode) {

        return $http.get(HomeService.AppUrl + 'ApprovedRequest/getApprovedRequest?EmployeeCode=' + EmployeeCode, {});

    };


});



app.controller('ApprovedRequestController', function ($scope, HomeService, RequestTypeService, $uibModal, PendingRequestService, ApprovedRequestService, $rootScope) {
    console.log("ApprovedRequest");


    $scope.ApprovedRequestList = [];

    $scope.init = function () {
        console.log($rootScope.session);

        $scope.ApprovedRequestGrid = true;
        $scope.ApprovedRequestForm = false;

        ApprovedRequestService.getApprovedRequest($rootScope.session.EMP_CODE).then(function (success) {

            $scope.ApprovedRequestList = success.data;
            return SalesReturnsService.GetLastThreeMonthDate();
        }).then(function (success) {

            $scope.LastDate = success.data;
        }, function (error) {
            console.log(error);
        });

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

    $scope.ViewRequestDetails = function (Id, CurrentStatus_Id, FutureStatus_Id, Obj) {

        $scope.CurrentStatus_Id = CurrentStatus_Id;
        $scope.Request_Id = Id;
        if (FutureStatus_Id == null) {

            $scope.FutureStatus_Id = 0;
        }
        else {

            $scope.FutureStatus_Id = FutureStatus_Id;
        }


        //Display purpose only

        $scope.TotalSRV_ = Obj.TotalSRV;
        $scope.CreatedDate_ = Obj.CreatedDate;
        $scope.CreatedBy_ = Obj.CreatedBy + ": " + Obj.CreatedBy_EMP_CODE;

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

        PendingRequestService.GetRequestDetails(Id, CurrentStatus_Id, $scope.FutureStatus_Id).then(function (success) {


            $scope.ApprovedRequestGrid = false;
            $scope.ApprovedRequestForm = true;

            $scope.ApprovedRequestDetails = success.data;



        }, function (error) {
            console.log(error);
        });


    };

    $scope.init();
});

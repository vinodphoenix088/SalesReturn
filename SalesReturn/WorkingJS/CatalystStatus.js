var app = angular.module('CatalystStatusModule', []);

app.service('CatalystStatusService', function ($http, $location, HomeService) {

    this.GetOpenRequest = function (empCode) {
        return $http.get(HomeService.AppUrl + 'CatalystStatus/GetOpenRequest?empCode=' + empCode, {});
    };

    this.GetClosedRequest = function (empCode) {
        return $http.get(HomeService.AppUrl + 'CatalystStatus/GetClosedRequest?empCode=' + empCode, {});
    };

    //this.GetRejectRequestCount = function (Id, EmployeeCode) {

    //    return $http.get(HomeService.AppUrl + 'Dashboard/GetRejectRequestCount?Id=' + Id + '&EmployeeCode=' + EmployeeCode, {});

    //};


});



app.controller('CatalystStatusController', function ($scope, CatalystStatusService, $rootScope) {
    $scope.Showp1 = true;
    console.log($rootScope.uid);
    console.log($rootScope.session);

    $scope.init = function () {

        CatalystStatusService.GetOpenRequest($rootScope.session.EMP_CODE).then(function (success) {
            console.log(success);
            $scope.OpenRequestList = success.data;
        });

    }

    $scope.GetOpenRequest = function () {
        $scope.Showp1 = true;
        CatalystStatusService.GetOpenRequest($rootScope.session.EMP_CODE).then(function (success) {
                console.log(success);
                $scope.OpenRequestList = success.data;
        });
    };

    $scope.GetClosedRequest = function () {
        $scope.Showp1 = false;
        CatalystStatusService.GetClosedRequest($rootScope.session.EMP_CODE).then(function (success) {
            console.log(success);
            $scope.ClosedRequestList = success.data;
        });

    };
    //$scope.GetOpenRequest();

    //$scope.init = function () {

    //    var i = $rootScope.Maininit();
    //    i.then(function (v) {

    //        console.log($rootScope.session);
    //        DashboardService.GetPendingRequestCount($rootScope.session.EMP_CODE).then(function (success) {
    //            console.log(success);
    //            $scope.PendingRequestCount = success.data;
    //        });

    //        DashboardService.GetInprocessRequestCount($rootScope.session.EMP_CODE).then(function (success) {
    //            console.log(success);
    //            $scope.InprocessRequestCount = success.data;
    //        });

    //        DashboardService.GetRejectRequestCount($rootScope.session.EMP_CODE).then(function (success) {
    //            console.log(success);
    //            $scope.RejectRequestCount = success.data;
    //        });

    //        DashboardService.GetApprovedRequestCount($rootScope.session.EMP_CODE).then(function (success) {
    //            console.log(success);
    //            $scope.ApprovedRequestCount = success.data;
    //        });

    //        //DashboardService.GetClosedRequestCount($rootScope.session.EMP_CODE).then(function (success) {
    //        //    console.log(success);
    //        //    $scope.ClosedRequestCount = success.data;
    //        //});
    //    },
    //        function (err) {
    //            console.log('error');
    //        });



    //};

    $scope.init();
});

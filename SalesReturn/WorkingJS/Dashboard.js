var app = angular.module('DashboardModule', []);

app.service('DashboardService', function ($http, $location, HomeService) {

    this.GetPendingRequestCount = function (empCode) {
        return $http.get(HomeService.AppUrl + 'Dashboard/GetPendingRequestCount?empCode=' + empCode, {});
    };
    this.GetInprocessRequestCount = function (empCode) {
        return $http.get(HomeService.AppUrl + 'Dashboard/GetInprocessRequestCount?empCode=' + empCode, {});
    };
    this.GetRejectRequestCount = function (empCode) {
        return $http.get(HomeService.AppUrl + 'Dashboard/GetRejectRequestCount?empCode=' + empCode, {});
    };
    this.GetClosedRequestCount = function (empCode) {
        return $http.get(HomeService.AppUrl + 'Dashboard/GetClosedRequestCount?empCode=' + empCode, {});
    };
    this.GetApprovedRequestCount = function (empCode) {
        return $http.get(HomeService.AppUrl + 'Dashboard/GetApprovedRequestCount?empCode=' + empCode, {});
    };
    this.GetSalesTotalRequestCount = function (empCode) {
        return $http.get(HomeService.AppUrl + 'Dashboard/GetSalesTotalRequestCount?empCode=' + empCode, {});
    };
    this.GetTotalRequestCount = function (empCode) {
        return $http.get(HomeService.AppUrl + 'Dashboard/GetTotalRequestCount?empCode=' + empCode, {});
    };
    this.GetPendingBillingClosureRequestCount = function (empCode) {
        return $http.get(HomeService.AppUrl + 'Dashboard/GetPendingBillingClosureRequestCount?empCode=' + empCode, {});
    };

    this.GetPendingSRVBillingClosureRequestCount = function (empCode) {
        return $http.get(HomeService.AppUrl + 'Dashboard/GetPendingSRVBillingClosureRequestCount?empCode=' + empCode, {});
    };

});



app.controller('DashboardController', function ($scope, DashboardService, $rootScope) {

    console.log($rootScope.uid);
    console.log($rootScope.session);


    $scope.init = function () {

        var i = $rootScope.Maininit();
        i.then(function (v) {

            console.log($rootScope.session);
            DashboardService.GetPendingRequestCount($rootScope.session.EMP_CODE).then(function (success) {
                console.log(success);
                $scope.PendingRequestCount = success.data;
            });

            DashboardService.GetInprocessRequestCount($rootScope.session.EMP_CODE).then(function (success) {
                console.log(success);
                $scope.InprocessRequestCount = success.data;
            });

            DashboardService.GetRejectRequestCount($rootScope.session.EMP_CODE).then(function (success) {
                console.log(success);
                $scope.RejectRequestCount = success.data;
            });

            DashboardService.GetApprovedRequestCount($rootScope.session.EMP_CODE).then(function (success) {
                console.log(success);
                $scope.ApprovedRequestCount = success.data;
            });

            DashboardService.GetClosedRequestCount($rootScope.session.EMP_CODE).then(function (success) {
                console.log(success);
                $scope.ClosedRequestCount = success.data;
            });

            DashboardService.GetSalesTotalRequestCount($rootScope.session.EMP_CODE).then(function (success) {
                console.log(success);
                $scope.SalesReqCount = success.data;
            });
            DashboardService.GetTotalRequestCount($rootScope.session.EMP_CODE).then(function (success) {
                console.log(success);
                $scope.TotalCount = success.data;
            });
            DashboardService.GetPendingBillingClosureRequestCount($rootScope.session.EMP_CODE).then(function (success) {
                console.log(success);
                $scope.PendingBillingRequestCount = success.data;
            });
            DashboardService.GetPendingSRVBillingClosureRequestCount($rootScope.session.EMP_CODE).then(function (success) {
                console.log(success);
                $scope.PendingSRVBillingRequestCount = success.data;
            });
        },
            function (err) {
                console.log('error');
            });

        

    };

    $scope.init();
});

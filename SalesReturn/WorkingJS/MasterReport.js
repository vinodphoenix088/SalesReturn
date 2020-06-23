var app = angular.module('MasterReporttModule', []);

app.service('MasterReportService', function ($http, $location, HomeService) {




    this.GetMasterReport = function (datefrom, dateTo) {

        return $http.get(HomeService.AppUrl + 'Common/GetMasterReport?datefrom=' + datefrom + '&dateTo=' + dateTo, {});

    };

});



app.controller('MasterReportController', function ($scope, HomeService, MasterReportService, RequestTypeService, $uibModal, PendingRequestService, InprocessRequestService, $rootScope, SalesReturnsService, $filter) {
    console.log("MasterReport");
    $scope.TodayDate = new Date();
    var date1 = new Date();
    var date2 = new Date();
    $scope.FromDatenull;
    $scope.ToDate = null;
    date1 = null;
    date2 = null;
    $scope.ShowReport = false;

    $scope.init = function () {
        MasterReportService.GetMasterReport('adf', 'abc').then(function (success) {
            $scope.ShowReport = true;
            $scope.ReprtData = success.data;
        });

    };
    $scope.GetReport = function () {
        if (!$scope.FromDate) { swal("info", "Please Select Form Date", "warning"); return false; }
        else if (!$scope.ToDate) { swal("info", "Please Select To Date", "warning"); return false; }
        MasterReportService.GetMasterReport($scope.FromDate, $scope.ToDate).then(function (success) {
            $scope.ShowReport = true;
            $scope.ReprtData = success.data;
        });

    };

    //$scope.init();
});

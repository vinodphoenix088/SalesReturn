var app = angular.module('ApprovalMatrixModule', []);

app.service('ApprovalMatrixService', function ($http, $location, HomeService) {


    this.getApprovalMatrixData = function () {

        return $http.get(HomeService.AppUrl + 'ApprovalMatrix/getApprovalMatrixData', {});

    };

    this.GetCountry = function (obj) {

        return $http.get(HomeService.AppUrl + 'Common/GetCountry', obj, {})

    };


    this.getBUForCountry = function (country) {

        return $http.get(HomeService.AppUrl + 'ApprovalMatrix/getBUForCountry?country=' + country)

    };
    this.getDivisionForBU = function (BU) {

        return $http.get(HomeService.AppUrl + 'ApprovalMatrix/getDivisionForBU?BU=' + BU)

    };

    this.GetSalesDirector = function (Obj) {

        return $http.post(HomeService.AppUrl + 'ApprovalMatrix/GetSalesDirector', Obj, {});

    };
    this.UpdateApprovalMatrix = function (Obj) {

        return $http.post(HomeService.AppUrl + 'ApprovalMatrix/UpdateApprovalMatrix', Obj, {});

    };

    this.DeleteApprovalMatrixRow = function (Id, EmployeeCode) {

        return $http.get(HomeService.AppUrl + 'ApprovalMatrix/DeleteApprovalMatrixRow?Id=' + Id + '&EmployeeCode=' + EmployeeCode, {});

    };


});



app.controller('ApprovalMatrixController', function ($scope, HomeService, RequestTypeService, ApprovalMatrixService, $rootScope) {
    console.log("ApprovalMatrix");
    $scope.CountryList = [];
    $scope.ApprovalMatrixTable = [];

    $scope.init = function () {

        ApprovalMatrixService.getApprovalMatrixData().then(function (success) {

            $scope.ApprovalMatrixTable = success.data;


            angular.forEach($scope.ApprovalMatrixTable, function (value, key) {
                $scope.getBUForCountry(value.Country, value);
                $scope.getDivisionForBU(value.BUType, value);
                $scope.GetSalesDirector(value);
            });

            return RequestTypeService.GetRequestTypeList();
        }).then(function (success) {

            $scope.RequestTypeList = success.data;

            angular.forEach($scope.ApprovalMatrixTable, function (value, key) {
                value.RequestTypeList = angular.copy(success.data);
            });

            return ApprovalMatrixService.GetCountry();
        }).then(function (success) {

            $scope.CountryList = success.data;

            angular.forEach($scope.ApprovalMatrixTable, function (value, key) {
                value.CountryList = angular.copy(success.data);
            });
        }, function (error) {
            console.log(error);
        });

    };


    $scope.getBUForCountry = function (country, Obj) {

        console.log(country);
        ApprovalMatrixService.getBUForCountry(country).then(function (success) {

            $scope.BUTypeList = success.data;

            Obj.BUTypeList = angular.copy(success.data);

        }, function (error) {
            console.log(error);
        });

    };


    $scope.getDivisionForBU = function (BU, Obj) {

        console.log(BU);
        ApprovalMatrixService.getDivisionForBU(BU).then(function (success) {

            $scope.DivisionList = success.data;

            Obj.DivisionList = angular.copy(success.data);

        }, function (error) {
            console.log(error);
        });

    };


    $scope.AddNewRow = function () {

        $scope.ApprovalMatrixTable.push({
            CountryList: $scope.CountryList,
            BUTypeList: $scope.BUTypeList,
            RequestTypeList: $scope.RequestTypeList
        });

    };


    $scope.GetSalesDirector = function (data) {

        if (data.Country && data.BUType && data.Division) {
            var Obj = {
                CountryName: data.Country,
                BUName: data.BUType,
                DivisionName: data.Division
            }
            ApprovalMatrixService.GetSalesDirector(Obj).then(function (success) {

                $scope.VPList = success.data;

                data.VPList = angular.copy(success.data);

                console.log(data);
            }, function (error) {
                console.log(error);
            });
        }

    };


    $scope.UpdateApprovalMatrix = function () {

        var check = true;

        angular.forEach($scope.ApprovalMatrixTable, function (value, key) {


            value.CreatedBy = $rootScope.session.EMP_CODE;

            if (!value.Country) {

                swal('Country can not be empty');
                check = false;
                return false;
            }
            else if (!value.BUType) {

                swal('BU Name can not be empty');
                check = false;
                return false;
            }
            else if (!value.Division) {

                swal('Division Name can not be empty');
                check = false;
                return false;
            }
            else if (!value.RequestType) {

                swal('Request Type can not be empty');
                check = false;
                return false;
            }
            else if (!value.SRV_Value) {

                swal('SRV value can not be empty');
                check = false;
                return false;
            }
            else if (!value.InvoiceAge) {

                swal('Invoice Age can not be empty');
                check = false;
                return false;
            }
            //else if (!value.ComplaintHandler) {

            //    swal('Complaint Handler can not be empty');
            //    check = false;
            //    return false;
            //}
            //else if (!value.ComplaintManager) {

            //    swal('Complaint Manager can not be empty');
            //    check = false;
            //    return false;
            //}
            //else if (!value.LogisticsManager) {

            //    swal('Logistic Manager can not be empty');
            //    check = false;
            //    return false;
            //}
            //else if (!value.LogisticsHead) {

            //    swal('Logistic Head can not be empty');
            //    check = false;
            //    return false;
            //}
            //else if (!value.SegmentHeadHRV) {

            //    swal('Segment Head HRV can not be empty');
            //    check = false;
            //    return false;
            //}
            //else if (!value.SegmentInvoiceAge) {

            //    swal('Segment Invoice age can not be empty');
            //    check = false;
            //    return false;
            //}
          
        });

        if (check) {
            console.log($scope.ApprovalMatrixTable);
            ApprovalMatrixService.UpdateApprovalMatrix($scope.ApprovalMatrixTable).then(function (success) {


                swal("Success", success.data, "success");
                $scope.init();

            }, function (error) {
                console.log(error);
            });
        }



    };



    $scope.RemoveRow = function (Id, index) {


        if (Id) {
            swal({
                title: "Are you sure?",
                text: "You want to delete?",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes",
                closeOnConfirm: false
            },
            function () {

                ApprovalMatrixService.DeleteApprovalMatrixRow(Id, $rootScope.session.EMP_CODE).then(function (success) {
                    $scope.init();
                    swal("Deleted!", success.data, "success");
                }, function (error) {

                    console.log(error);

                });
            });

        }
        else {
            $scope.ApprovalMatrixTable.splice(index, 1);
        }
        console.log($scope.ApprovalMatrixTable);
    };


    $scope.init();
});

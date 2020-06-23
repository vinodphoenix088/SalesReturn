

var app = angular.module('EmployeeModule', []);

app.service('EmployeeService', function ($http, $location, HomeService) {


    this.getEmployeeList = function () {

        return $http.get(HomeService.AppUrl + 'AdminMaster/getEmployeeList', {});

    };

    this.GetCCStackHolderDetail = function () {

        return $http.get(HomeService.AppUrl + 'AdminMaster/GetCCStackHolderDetail', {});

    }

    this.DeleteList = function (Id, EmployeeCode) {

        return $http.get(HomeService.AppUrl + 'AdminMaster/DeleteList?Id=' + Id + '&EmployeeCode=' + EmployeeCode, {});

    };

    this.UpdateEmployeeList = function (Emplist) {

        return $http.post(HomeService.AppUrl + 'AdminMaster/UpdateEmployeeList', Emplist, {});

    };


});



app.controller('EmployeeController', function ($scope, HomeService, RequestTypeService, SalesReturnsService, EmployeeService, $rootScope) {
    console.log("AdminMaster");
    $scope.AdminList = [];

    $scope.init = function () {

       


        EmployeeService.getEmployeeList().then(function (success) {

            $scope.EmployeeList = success.data;
            console.log($scope.EmployeeList);

            //

           

            //


        }, function (error) {
            console.log(error);
        });
          
        SalesReturnsService.GetDepotList('0').then(function (success) {

            if (success.data != null) {
                $scope.DepotList = success.data;
                
            }

            return EmployeeService.GetCCStackHolderDetail()
        }).then(function (success) {
            $scope.CCStackHolders = success.data;
            

        }, function (error) {
            console.log(error);

        });
    };

    $scope.getDepotPerson = function (pt) {


        console.log(pt);

        EmployeeService.getDepotPersonCode().then(function (success) {

            $scope.EmployeeList = success.data;

        }, function (error) {
            console.log(error);
        });
    }



    $scope.AddNewRow = function () {

        $scope.EmployeeList.push({

            
        });

    };


    $scope.UpdateEmployeeList = function (EmployeeList) {
        console.log($scope.EmployeeList);
        var check = true;

        angular.forEach($scope.EmployeeList, function (value, key) {


            value.CreatedBy = $rootScope.session.EMP_CODE;

            if (!value.DepotName) {

                swal('Depot Name can not be empty');
                check = false;
                return false;
            }
            if (!value.Depotcode) {

                swal('Depot Person Code can not be empty');
                check = false;
                return false;
            }
            if (!value.CommercialCode) {

                swal('Commercial Emp Code can not be empty');
                check = false;
                return false;
            }
            if (!value.CSO) {

                swal('CSO Emp Code can not be empty');
                check = false;
                return false;
            }
            if (!value.ComplaintHandler) {

                swal('please select  Customer Complaint Handler');
                check = false;
                return false;
            }
            if (!value.ComplaintManager) {

                swal('please select Customer Complaint Manager');
                check = false;
                return false;
            }
            if (!value.LogisticsHead) {

                swal('Logistic Manager can not be empty');
                check = false;
                return false;
            }
            if (!value.ISC) {

                swal('ISC Head can not be empty');
                check = false;
                return false;
            }

        });

        

        if (check) {
            console.log($scope.EmployeeList);
            EmployeeService.UpdateEmployeeList(EmployeeList).then(function (success) {


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

                    EmployeeService.DeleteList(Id, $rootScope.session.EMP_CODE).then(function (success) {
                        $scope.init();
                        swal("Deleted!", success.data, "success");
                    }, function (error) {

                        console.log(error);

                    });
                });

        }
        else {
            $scope.EmployeeList.splice(index, 1);
        }
        console.log($scope.EmployeeList);
    };


    $scope.init();
});

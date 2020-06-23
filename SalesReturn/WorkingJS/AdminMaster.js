var app = angular.module('AdminMasterModule', []);

app.service('AdminMasterService', function ($http, $location, HomeService) {


    this.getAdminList = function () {

        return $http.get(HomeService.AppUrl + 'AdminMaster/getAdminList', {});

    };
    this.UpdateAdminList = function (Obj) {

        return $http.post(HomeService.AppUrl + 'AdminMaster/UpdateAdminList', Obj, {});

    };

    this.DeleteAdminList = function (Id, EmployeeCode) {

        return $http.get(HomeService.AppUrl + 'AdminMaster/DeleteAdminList?Id=' + Id + '&EmployeeCode=' + EmployeeCode, {});

    };


});



app.controller('AdminMasterController', function ($scope, HomeService, RequestTypeService, AdminMasterService, $rootScope) {
    console.log("AdminMaster");
    $scope.AdminList = [];

    $scope.init = function () {

        AdminMasterService.getAdminList().then(function (success) {

            $scope.AdminList = success.data;

        }, function (error) {
            console.log(error);
        });

    };



    $scope.AddNewRow = function () {

        $scope.AdminList.push({
           
            EmployeeCode : ""
        });

    };


    $scope.UpdateAdminList = function () {

        var check = true;

        angular.forEach($scope.AdminList, function (value, key) {


            value.CreatedBy = $rootScope.session.EMP_CODE;

            if (!value.EmployeeCode) {

                swal('Employee Code can not be empty');
                check = false;
                return false;
            }

        });

        if (check) {
            console.log($scope.AdminList);
            AdminMasterService.UpdateAdminList($scope.AdminList).then(function (success) {


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

                AdminMasterService.DeleteAdminList(Id, $rootScope.session.EMP_CODE).then(function (success) {
                    $scope.init();
                    swal("Deleted!", success.data, "success");
                }, function (error) {

                    console.log(error);

                });
            });

        }
        else {
            $scope.AdminList.splice(index, 1);
        }
        console.log($scope.AdminMasterTable);
    };


    $scope.init();
});

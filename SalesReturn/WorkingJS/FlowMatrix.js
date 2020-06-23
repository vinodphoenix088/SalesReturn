

var app = angular.module('FlowMatrixModule', []);

app.service('FlowMatrixService', function ($http, $location, HomeService) {


    this.getFlowMatrixList = function () {

        return $http.get(HomeService.AppUrl + 'ApprovalMatrix/getFlowMatrixList', {});

    };
    this.UpdateFlowList = function (Obj) {

        return $http.post(HomeService.AppUrl + 'ApprovalMatrix/UpdateFlowList', Obj, {});

    };

    this.DeleteAdminList = function (Id, EmployeeCode) {

        return $http.get(HomeService.AppUrl + 'ApprovalMatrix/DeleteAdminList?Id=' + Id + '&EmployeeCode=' + EmployeeCode, {});

    };


});



app.controller('FlowMatrixController', function ($scope, HomeService, RequestTypeService, ReasonService, ReasonMasterService , FlowMatrixService, $rootScope) {
    console.log("AdminMaster");
    $scope.AdminList = [];

    $scope.init = function () {

        RequestTypeService.GetRequestTypeList().then(function (success) {
            console.log(success);
            $scope.RequestTypeList = success.data;
        });

        FlowMatrixService.getFlowMatrixList().then(function (success) {

            $scope.FlowList = success.data;

        }, function (error) {
            console.log(error);
        });

    };



    $scope.AddNewRow = function () {

        $scope.AdminList.push({

            EmployeeCode: ""
        });

    };


    $scope.UpdateFlowMatrix = function (FlowList) {

        var check = true;
        console.log(FlowList);

      

        if (check) {
            console.log(FlowList);
            FlowMatrixService.UpdateFlowList(FlowList).then(function (success) {


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

                    FlowMatrixService.DeleteAdminList(Id, $rootScope.session.EMP_CODE).then(function (success) {
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

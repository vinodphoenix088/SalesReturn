var app = angular.module('CSOReasonModule', []);

app.service('CSOReasonService', function ($http, $location, HomeService) {

    this.GetSAPReasonDetail = function () {

        return $http.get(HomeService.AppUrl + 'ReasonMaster/GetSAPReasonDetail', {});
    };

    this.UpdateSAPSubReason = function (Obj) {

        return $http.post(HomeService.AppUrl + 'ReasonMaster/UpdateSAPSubReason', Obj, {});

    };

    this.DeleteSAPSubReason = function (id, EmpCode) {


        return $http.get(HomeService.AppUrl + 'ReasonMaster/DeleteSAPSubReason?Id=' + id + '&EmployeeCode=' + EmpCode, {});

    };


    this.GetSAPSubReasonMasterList = function () {

        return $http.get(HomeService.AppUrl + 'ReasonMaster/GetSAPSubReasonMasterList', {});
    };


});



app.controller('CSOReasonController', function ($scope, HomeService, RequestTypeService, CSOReasonService, ReasonMasterService, ReasonService, $rootScope) {
    console.log("Reason");


    $scope.init = function () {

        $scope.showUpload = false;

        CSOReasonService.GetSAPReasonDetail().then(function (success) {
            console.log(success);
            $scope.ReasonDeatil = success.data;
        });

        CSOReasonService.GetSAPSubReasonMasterList().then(function (success) {
            console.log(success);
            $scope.ReasonDeatilSAP = success.data;
        });

        ReasonMasterService.GetReasonMasterList().then(function (success) {

            if (success.data != null) {
                $scope.RequestTypeList = success.data;

            }

        }, function (error) {

            console.log(error);

        });
    };




    $scope.AddNewRow = function () {

        console.log("IN addition");
        $scope.ReasonDeatil.push({
            Reason: null,
            SubReason: '',

        });

    };


    $scope.Update = function () {

        var check = true;

        angular.forEach($scope.ReasonDeatil, function (value, key) {


            value.CreatedBy = $rootScope.session.EMP_CODE;

            if (!value.RequestTypeId) {


                swal('Please Select Reason');
                check = false;
                return false;
            }

            else if (!value.SubReasonID) {


                swal('SubReason can not be empty');
                check = false;
                return false;
            }





        });

        if (check) {
            console.log($scope.ReasonDeatil);
            CSOReasonService.UpdateSAPSubReason($scope.ReasonDeatil).then(function (success) {
                if (success.data === 'Success : Reasons succssfully saved') {
                    swal("Success", success.data, "success");

                }
                else {
                    swal("Error", success.data, "error");
                }


                $scope.init();

            }, function (error) {
                console.log(error);
            });
        }



    };



    $scope.RemoveRow = function (id, index) {

        console.log(id);

        if (id) {
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

                    if (id === null) {
                        $scope.ReasonDeatil.splice(index, 1);
                    }
                    else {
                        CSOReasonService.DeleteSAPSubReason(id, $rootScope.session.EMP_CODE).then(function (success) {
                            $scope.init();
                            swal("Deleted!", success.data, "success");
                        }, function (error) {

                            console.log(error);

                        });

                    }

                });

        }
        else {
            $scope.ReasonDeatil.splice(index, 1);
        }
        console.log($scope.RequestTypeList);
    };


    $scope.init();
});

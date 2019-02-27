(function () {
    //var app = angular.module('app', []);
    app.controller('homeController', homeController);

    homeController.$inject = ['homeDataContext'];

    function homeController(homeDataContext) {
        var vm = this;
        vm.datas = [];
        vm.fileds = [];
        vm.data = {
            fields: {}
        };
        vm.keyword = "";
        vm.CreatedCount = 0;
        vm.getList = getList;

        vm.showGenerated = false;
        vm.submittedForm = false;
        
        getList();
        function getList() {
            var keyword = angular.copy(vm.keyword);
            homeDataContext.get(keyword).then(function (data) {
                vm.datas = data;
                console.log(data);
            });
        }

        vm.generate = generate;
        function generate() {
            vm.showGenerated = true;
            var count = angular.copy(vm.CreatedCount);
            var arr = [];
            for (let i = 0; i < count; i++) {
                arr.push({ 'required': true, 'name': i + '.name', 'dataType': "text" });
            }
            vm.fileds = arr;
        }
      

        vm.saveForm = saveForm;
        function saveForm(form) {
            if (form.$invalid) {
                vm.submittedForm = true;
                return false;
            }
            save();
        }

        function save() {
            var fields = angular.copy(vm.fileds);
            vm.data.fields = fields;
            var data = angular.copy(vm.data);

            homeDataContext.add(data).then(function (_data) {
                console.log(_data);
                var data = JSON.parse(_data.data);
                if (data.status === false) {
                    
                    alert(data.Message + "\n" + data.Result);
                }
                else {
                    alert(data.Message + "\n" + data.Result);
                    vm.data = [];
                    vm.fields = [];
                    vm.showGenerated = false;
                    getList();
                }
           
            });
        }
       
        vm.DeleteForm = DeleteForm;

        function DeleteForm(form) {
            var frm = angular.copy(form);
            
            homeDataContext.delete(frm.id).then(function (_data) {
                var data = JSON.parse(_data);
                if (data.status===false) {
                    alert(data.Message+"\n"+data.Result);
                }
                else {
                    alert(data.Message + "\n" + data.Result);
                }
                getList();
            });
        }



    }

})();
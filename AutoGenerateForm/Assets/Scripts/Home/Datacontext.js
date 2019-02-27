app.factory('homeDataContext', homeDataContext);
homeDataContext.$inject = ['$http'];

function homeDataContext($http) {
    return {
        get: _get,
        add: _add,
        delete: _delete
    };

    function _get(keyword) {
        return $http.get('/Home/GetAllForms?keyword='+keyword).then(completed);
    }

    function completed(response) {
        return response.data;
    }

    function _add(form) {
        return $http.post('/Home/AddForm',form);
    }

    function _delete(id) {
        return $http.get('/Home/DeleteForm?id=' + id).then(completed);
    }
}
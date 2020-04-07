(function ($) {
    function UserProfile() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-userprofile").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new UserProfile();
        self.init();
    })
}(jQuery))
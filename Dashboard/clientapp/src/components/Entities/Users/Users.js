//import addUsers from './AddUsers/AddUsers.vue';
//import editUsers from './EditUsers/EditUsers.vue';
import moment from 'moment';

export default {
    name: 'Customers',
    created() {

        this.getEntityUser(this.$parent.EnitiesSelectedId);

    },
    components: {
        //'add-Users': addUsers,
        //'edit-Users': editUsers
    },
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
            // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format('MMMM Do YYYY');
        }
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 5,
            pages: 0,
            state: 0,
            users:[],

            EditUsersObj: [],
        };
    },
    methods: {

         

        getEntityUser(id) {

            this.$blockUI.Start();
            this.$http.getEntityUser(id)
                .then(response => {
                    this.$blockUI.Stop();
                    this.users = response.data.users;
                    this.pages = response.data.count;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                   // console.error(err);
                });

        },

        addUser() {
            this.state = 1;
        },

        EditUser(User) {
            this.state = 2;
            this.EditUsersObj = User;

        },

        DeactivateUser(UserId) {


            this.$confirm('سيؤدي ذلك إلى ايقاف تفعيل المستخدم  . استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {


                this.$http.DeactivateEntityUser(UserId)
                    .then(() => {
                        if (this.users.lenght === 1) {
                            this.pageNo--;
                            if (this.pageNo <= 0) {
                                this.pageNo = 1;
                            }
                        }
                        this.$message({
                            type: 'info',
                            message: 'تم ايقاف التفعيل المستخدم بنجاح',
                        });
                        this.getUser();
                    })
                    .catch((err) => {
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });
            });
        },

        ActivateUser(UserId) {

            this.$confirm('سيؤدي ذلك إلى تفعيل المستخدم  . استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {


                this.$http.ActivateEntityUser(UserId)
                    .then(() => {
                        if (this.users.lenght === 1) {
                            this.pageNo--;
                            if (this.pageNo <= 0) {
                                this.pageNo = 1;
                            }
                        }
                        this.$message({
                            type: 'info',
                            message: 'تم تفعيل المستخدم بنجاح',
                        });
                        this.getUser();
                    })
                    .catch((err) => {
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });
            });

        },

        delteUser(UserId) {

            this.$confirm('سيؤدي ذلك إلى حدف المستخدم  . استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {


                this.$http.delteEntityUser(UserId)
                    .then(() => {
                        if (this.users.lenght === 1) {
                            this.pageNo--;
                            if (this.pageNo <= 0) {
                                this.pageNo = 1;
                            }
                        }
                        this.$message({
                            type: 'info',
                            message: 'تم حدف المستخدم بنجاح',
                        });
                        this.getUser();
                    })
                    .catch((err) => {
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });
            });

        },



        Back() {
            this.$parent.state = 0;
        }

    }
}

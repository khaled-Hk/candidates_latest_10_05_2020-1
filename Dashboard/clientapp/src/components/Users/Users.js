import addUsers from './AddUsers/AddUsers.vue';
import editUsers from './EditUsers/EditUsers.vue';
import moment from 'moment';

export default {
    name: 'Customers',
    created() {

        this.SearchType = [
            {
                id: 1,
                name: "الإسم"
            },
            {
                id: 2,
                name: 'رقم الخدمة'
            }, {
                id: 3,
                name: "الحالة"
            }


        ];

        this.pakegeStatus = [
            {
                id: 1,
                name: "Active"
            },
            {
                id: 2,
                name: 'Not Active'
            }, {
                id: 3,
                name: "Done"
            }


        ];

        this.getUser();

    },
    components: {
        'add-Users': addUsers,
        'edit-Users': editUsers
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

         

        getUser(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }

            this.$blockUI.Start();
            this.$http.getUser(this.pageNo, this.pageSize)//this.$parent.SuperPackageParent.superPackageId
                .then(response => {
                    this.$blockUI.Stop();
                    this.users = response.data.users;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
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


                this.$http.DeactivateUser(UserId)
                    .then(response => {
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


                this.$http.ActivateUser(UserId)
                    .then(response => {
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


                this.$http.delteUser(UserId)
                    .then(response => {
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
    }
}

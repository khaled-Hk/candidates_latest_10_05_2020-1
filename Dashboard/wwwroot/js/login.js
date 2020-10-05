var app = new Vue({
    el: '#app2',
    name: 'login',
    created() {
        this.returnurl = location.pathname;
        this.SetRules();
    },
    data() {
        return {
            returnurl: '',
            form: {
                Password: null,
                Email: null
            },
            form2: {
                Email: null
            },
            success: { confirmButtonText: 'OK', type: 'success', dangerouslyUseHTMLString: true, center: true },
            error: { confirmButtonText: 'OK', type: 'error', dangerouslyUseHTMLString: true, center: true },
            warning: { confirmButtonText: 'OK', type: 'warning', dangerouslyUseHTMLString: true, center: true },
            rules: {},
            forgetPassowrd: false,



        };
    },
    methods: {

        login() {
            if (!this.form.Email) {
                swal({
                    title: "خطأ",
                    text: 'الرجاء إدخال لبريد الإلكتروني',
                    icon: "error",
                });
                return;
            }
            if (!this.form.Password) {
                swal({
                    title: "خطأ",
                    text: 'الرجاء إدخال الرقم السري',
                    icon: "error",
                });
                return;
            }

            axios.post(`/Security/loginUser`, this.form)
                .then(response => {
                    ///   $blockUI.close();
                    sessionStorage.setItem('currentUser', JSON.stringify(response.data));
                    window.location.href = '/';
                })
                .catch((error) => {
                    //$blockUI.close();
                    swal({
                        title: "خطأ",
                        text: error.response.data,
                        icon: "error",
                    });

                });
        },


        ResetPassword(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    let $blockUI = this.$loading({
                        fullscreen: true,
                        text: 'loading ...'
                    });
                    this.$http.ResetPassword(this.form2.Email.trim())
                        .then(response => {
                            $blockUI.close()
                            this.form2.Email = null;
                            this.forgetPassowrd = false;
                            this.$alert('<h4>' + response.data + '</h4>', '', this.success);
                        })
                        .catch((error) => {
                            $blockUI.close()

                            if (error.response.status == 400) {
                                this.$alert('<h4>' + error.response.data + '</h4>', '', this.warning);
                            } else if (error.response.status == 404) {
                                this.$alert('<h4>' + error.response.data + '</h4>', '', this.error);
                            } else {
                                console.log(error.response);
                            }
                        });
                } else {
                    console.log("form not complete");
                    return false;
                }
            });
        },
        SetRules() {

            this.rules = {
                Email: [
                    { required: true, message: 'Please input your Email', trigger: 'blur' },
                    { type: 'email', message: 'Please input correct email address', trigger: ['blur', 'change'] }
                ],
                Password: [
                    { required: true, message: 'Please input your Password', trigger: 'blur' }
                ],
            }
        },

    }

});

import moment from 'moment';

export default {
    name: 'CompleteRegistration',
    created() {
       this.fetchCandidateData();
    },
    components: {

    },
    data() {
        return {
            candidate: null

        };
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
    methods: {

        fetchCandidateData() {
            


            this.$blockUI.Start();
            this.$http.GetCandidateInfo(this.$parent.Nid)
                .then((response) => {
                  
                    this.$blockUI.Stop();
                    this.candidate = response.data;
                   
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$notify({
                        title: 'خطأ بعملية الاضافة',
                        dangerouslyUseHTMLString: true,
                        type: 'error',
                        message: err.response.data
                    });
                });
        
        },

        printReport() {

            let reportHTML = `
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="ie=edge" />
    <style>
@import url("https://fonts.googleapis.com/css2?family=Tajawal:wght@200;300;400;500;700;800;900&display=swap");
@import url("https://fonts.googleapis.com/css2?family=Cairo:wght@200;300;400;600;700;900&display=swap");

html{
background: white;
  display: block;
  margin: 0 auto;
}
body {
  background: #f5f6fa;
  font-family: "Tajawal", sans-serif;
  font-family: "Cairo", sans-serif;
  direction: rtl;
}

.form-box-one {
  float: right;
  width: 1080px;
  aheight: 620px;
  padding-top: 10px;
  background-color: white;
  margin: 100px calc(50% - 540px);
  box-shadow: 0 5px 10px rgb(0 0 0 / 7%);
  background-image: url(https://i.ibb.co/kSmW8MM/22.jpg);
  background-size: cover;
}

.form-box-one .header {
  float: right;
  width: calc(100% - 40px);
  margin: 5px 20px;
  abackground-color: blue;
}

.form-box-one .header img {
  float: right;
  height: 80px;
  margin: 5px 0px;
}

.form-box-one .header .re-title {
  float: right;
  height: 80px;
  margin: 5px 0px;
  min-width: 510px;
  text-align: center;
  font-size: 30px;
  line-height: 75px;
  abackground: orange;
  font-weight: 500;
  color: #444444;
}

.line {
  float: right;
  width: 100%;
  height: 1px;
  background-color: #9a9a9a;
}

.form-box-one .content {
  float: right;
  margin: 30px 40px;
  width: calc(100% - 80px);
  abackground-color: #aaaaaa;
  padding-bottom: 10px;
}

.item-1 {
  float: right;
  width: 100%;
  height: 46px;
  margin: 20px 0px;
  abackground: blue;
}

.item-2 {
  float: right;
  width: 50%;
  height: 46px;
  margin: 20px 0px;
  abackground: blue;
}

.item-3 {
  float: right;
  width: calc(100% / 3);
  height: 46px;
  margin: 20px 0px;
  abackground: blue;
}

.label {
  float: right;
  line-height: 46px;
  color: #333333;
  font-weight: 500;
  font-size: 19px;
}

.input {
  float: right;
  margin-right: 40px;
  line-height: 46px;
  font-size: 19px;
  color: #333333;
  border: 1px solid #eeeeee;
  padding: 0px 25px;
  border-radius: 7px;
}

.footer {
  float: right;
  background: #8a8a8a;
  height: 76px;
  width: calc(100% - 40px);
  padding: 0px 20px;
}

.footer .em-number {
  float: right;
  line-height: 76px;
  font-size: 19px;
  color: #555555;
}

    </style>
    <title></title>
  </head>
  <body class="form-box-one">
    <!-- Form One -->
    <div class="">
      <!-- Header -->
      <div class="header">
        <img
          src="https://hnec.ly/wp-content/uploads/2020/06/LOGOsss.png"
          alt=""
        />
        <div class="re-title">
          إيصال تسجيل مرشح
        </div>
      </div>
      <!-- End Header -->
      <div class="line"></div>

      <!-- Content -->
      <div class="content">
        <!-- Item -->
        <div class="item-1">
          <div class="label">التاريخ</div>
          <div class="input">${this.candidate.createdOn}</div>
        </div>
        <!-- End Item -->

        <!-- Item -->
        <div class="item-2">
          <div class="label">إسم المرشح</div>
          <div class="input">${this.candidate.name}</div>
        </div>
        <!-- End Item -->

        <!-- Item -->
        <div class="item-2">
          <div class="label">الرقم الوطني</div>
          <div class="input">${this.candidate.nid}</div>
        </div>
        <!-- End Item -->

        <!-- Item -->
        <div class="item-2">
          <div class="label">الدائرة الرئيسية</div>
          <div class="input">${this.candidate.constituencyName}</div>
        </div>
        <!-- End Item -->

        <!-- Item -->
        <div class="item-2">
          <div class="label">الدائرة الفرعية</div>
          <div class="input">${this.candidate.subconstituencyName}</div>
        </div>
        <!-- End Item -->

        <!-- Item -->
        <div class="item-2">
          <div class="label">المكتب الإنتخابي</div>
          <div class="input">${this.candidate.officaeName}</div>
        </div>
        <!-- End Item -->

        <!-- Item -->
        <div class="item-2">
          <div class="label">رقم التسجيل</div>
          <div class="input">${this.candidate.candidateId}</div>
        </div>
        <!-- End Item -->
      </div>
      <!-- End Content -->

      <!-- Footer  -->
      <div class="footer">
        <div class="em-number">موظف التسجيل: ${this.candidate.officeUserId}</div>
      </div>
      <!-- End Footer  -->
    </div>
    <!-- End Form One -->
  </body>
</html>
               `;
           const WinPrint = window.open();
            let is_chrome = Boolean(WinPrint.chrome);
            WinPrint.document.write(reportHTML);
            if (is_chrome) {
                setTimeout(function () {
                    WinPrint.document.close();
                    WinPrint.focus();
                    WinPrint.print();
                }, 250);
            } else {
                WinPrint.document.close();
                WinPrint.focus();
            }
        }
        

    }

}
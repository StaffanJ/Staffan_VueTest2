<template>
    <div class="container">
        <div class="row">
            <div class="col-2">
                <button type="submit" class="btn" @click.prevent="fetchEmployee()"><img src="../icon/house.svg"></button>
            </div>
            <div class="col-10">

            </div>
            
        </div>
        <div class="row">
            <div v-if="EmployeeGet" class="col" id="employeesGet">
                <table class="table table-hover table-striped table-bordered caption-top">
                    <caption>List of employees</caption>
                    <thead class="table-dark">
                        <tr>
                            <th>#</th>
                            <!--<th>Id</th>-->
                            <th>Firstname</th>
                            <th>Lastname</th>
                            <th>Comment</th>
                            <th>Is available: {{ EmpIsAvailable }}</th>
                            <th>Edit</th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- Hämtar alla anställda, kopplad till fetchEmployee() funktionen.-->
                        <template v-for="emp in EmployeeGet" :key="emp.id">
                            <tr>
                                <td><img src="../icon/person-square.svg"></td>
                                <td>{{ emp.firstName }}</td>
                                <td>{{ emp.lastName }}</td>
                                <td>{{ emp.comment }}</td>
                                <td v-if="emp.isAvailable === 0" id="employeeStatusHover">
                                    <!-- Skickar in till funktionen changeStatusEmployee, skickar med id och om den anställda är tillgänglig.-->
                                    <button type="submit" class="btn" @click.prevent="changeStatusEmployee(emp.id, emp.isAvailable)"><img src="../icon/exclamation-circle.svg"></button>
                                </td>
                                <td v-if="emp.isAvailable === 1" id="employeeStatusHover">
                                    <!-- Skickar in till funktionen changeStatusEmployee, skickar med id och om den anställda är tillgänglig.-->
                                    <button type="submit" class="btn" @click.prevent="changeStatusEmployee(emp.id, emp.isAvailable)"><img src="../icon/exclamation-circle-fill.svg"></button>
                                </td>
                                <td><button type="submit" class="btn" @click.prevent="changeEmployeeInformation(emp.id, emp.firstName, emp.lastName, emp.age, emp.comment)"><img src="../icon/pencil-square.svg"></button></td>
                            </tr>
                        </template>
                    </tbody>
                </table>
            </div>
            <!-- Hämtar en anställd, kopplad till rollEmployee() funktionen.-->
            <div v-if="EmployeeRollShow" class="col" id="employeesPost">
                <table class="table table-bordered caption-top">
                    <caption>Employee for the next task</caption>
                    <thead class="table-dark">
                        <tr>
                            <th></th>
                            <th>Name</th>
                            <th>Number of times</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-if="EmployeeRoll">
                            <td><img src="../icon/person-workspace.svg" /></td>
                            <td>{{ EmployeeRoll.firstName }}</td>
                            <td>{{ EmployeeRoll.numOfTimes }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>

        </div>

        <div v-if="EmployeeGet" class="row">
            <div id="employeeRoll" class="col">
                <div>
                    <form @submit.prevent="rollEmployee">
                        <button type="submit" class="btn btn-outline-info btn-sm">Who gets to be the "lucky" one?</button>
                    </form>
                </div>
            </div>
        </div>

        <div v-if="EmployeeEdit" class="row" id="employeeEdit">
            <div class="col-4">

            </div>
            <div class="col-2">
                <p>Firstname:</p>
                <p>Lastname:</p>
                <p>Age:</p>
                <p>Comment:</p>
            </div>
            <div class="col-2">
                <input type="text" id="employeeEditInput" v-model="employeeEditInfo.firstName" /><br />
                <input type="text" id="employeeEditInput" v-model="employeeEditInfo.lastName" /><br />
                <input type="text" id="employeeEditInput" v-model="employeeEditInfo.age" /><br />
                <textarea  v-model="employeeEditInfo.comment" rows="4"></textarea>
            </div>
            <div class="col-4">

            </div>
            <form @submit.prevent="changeEmployeeInformationPost">
                <button type="submit" class="btn btn-outline-info btn-sm">Submit changes.</button>
            </form>
        </div>

    </div>

</template>

<script lang="js">
    import { defineComponent } from 'vue';

    export default defineComponent({
        // Data vi använder i denna komponenten, denna delen är jag lite osäker på fortfarande.
        data() {
            return {
                EmployeeGet: null,
                EmployeePost: null,
                EmployeeRoll: null,
                Employeeloading: false,
                EmployeeEdit: false,
                EmployeeRollShow: false,
                EmpIsAvailable: 0,

                employeeEditInfo: {
                    id: 0,
                    firstName: "",
                    lastName: "",
                    age: 0,
                    comment: ""
                }
            };
        },
        // Körs när applikationen startar.
        async created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchEmployee();
        },
        // !!!!!!!!!!!!!!!! Ta reda på mer om watch.
        watch: {
            // call again the method if the route changes
            '$route': 'fetchEmployee'
        },
        // Metoderna som vi kan använda oss av och kalla på.
        methods: {
            // Hämtar alla anställda och retunerar dem.
            fetchEmployee(){
                this.EmployeeGet = null;
                this.Employeeloading = true;
                this.EmployeeEdit = false;

                fetch('Employee')
                    .then(r => r.json())
                    .then(json => {
                        this.EmployeeGet = json;
                        this.Employeeloading = false;

                        this.EmployeeGet.forEach((value, index) => {
                            if (value.isAvailable) {
                                this.increment();
                            }
                        });

                        return;
                    })
                    .catch(err => {
                        err
                    });;
            },
            // Ändrar om en anställd är tillgänglig för ett request.
            changeStatusEmployee(employee, isAvailable) {
                this.EmployeePost = null;
                this.EmpIsAvailable = 0;

                fetch('Employee/ChangeStatus/', {
                    method: 'POST',
                    headers: {
                        'Content-type': 'application/json'
                    },
                        body: JSON.stringify({ id: employee, isavailable: isAvailable })
                    })
                    .then(r => r.json())
                    .then(json => {
                        this.EmployeePost = json;
                        this.fetchEmployee();
                        return;
                    })
                    .catch(err => {
                        err
                    });
            },
            // Hämtar nästa anställd som ska hantera uppgiften.
            rollEmployee() {
                this.EmployeeRoll = null;
                this.EmployeeRollShow = true;

                fetch('Employee/Roll', {
                })
                .then(r => r.json())
                .then(json => {
                    this.EmployeeRoll = json;
                    this.fetchEmployee();
                    return;
                })
                .catch(err => {
                    err
                });
            },
            // Tar fram vyn för att ändra information.
            changeEmployeeInformation(id, firstName, lastName, age, comment) {
                this.EmployeeGet = null;
                this.EmployeePost = null;
                this.EmployeeRoll = null;
                this.EmployeeEdit = true;

                this.employeeEditInfo.id = id;
                this.employeeEditInfo.firstName = firstName;
                this.employeeEditInfo.lastName = lastName;
                this.employeeEditInfo.age = age;
                this.employeeEditInfo.comment = comment
            },
            // Tar fram vyn för att ändra information 
            changeEmployeeInformationPost() {
                fetch('Employee/ChangeInformation/', {
                    method: 'POST',
                    headers: {
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify({
                        id: this.employeeEditInfo.id,
                        firstName: this.employeeEditInfo.firstName,
                        lastName: this.employeeEditInfo.lastName,
                        age: this.employeeEditInfo.age,
                        comment: this.employeeEditInfo.comment
                    })
                })
                    .then(r => r.json())
                    .then(json => {
                        this.EmpIsAvailable = 0;
                        this.fetchEmployee();
                        return;
                    })
                    .catch(err => {
                        err
                    });
                
            },
            increment() {
                this.EmpIsAvailable += 1;
            }
        },
    });

</script>

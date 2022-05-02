<template>
    <div class="container">
        <div class="row">


            <div v-if="EmployeeGet" class="col" id="employeesGet">
                <table class="table table-hover table-striped table-bordered caption-top">
                    <caption>List of employees</caption>
                    <thead class="table-dark">
                        <tr>
                            <th></th>
                            <!--<th>Id</th>-->
                            <th>Firstname</th>
                            <th>Lastname</th>
                            <th>Number of times</th>
                            <th>Is available</th>
                            <th>Edit</th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- Hämtar alla anställda, kopplad till fetchEmployee() funktionen.-->
                        <tr v-for="emp in EmployeeGet" :key="emp.id">
                            <td><img src="../icon/person-square.svg"></td>
                            <td>{{ emp.firstName }}</td>
                            <td>{{ emp.lastName }}</td>
                            <td>{{ emp.numOfTimes }}</td>
                            <td v-if="emp.isAvailable === 0" id="employeeStatusHover">
                                <!-- Skickar in till funktionen changeStatusEmployee, skickar med id och om den anställda är tillgänglig.-->
                                <button type="submit" class="btn" @click.prevent="changeStatusEmployee(emp.id, emp.isAvailable)"><img src="../icon/exclamation-circle.svg"></button>
                            </td>
                            <td v-if="emp.isAvailable === 1" id="employeeStatusHover">
                                <!-- Skickar in till funktionen changeStatusEmployee, skickar med id och om den anställda är tillgänglig.-->
                                <button type="submit" class="btn" @click.prevent="changeStatusEmployee(emp.id, emp.isAvailable)"><img src="../icon/exclamation-circle-fill.svg"></button>
                            </td>
                            <td><button type="submit" class="btn" @click.prevent="changeEmployeeInformation(emp.id, emp.firstName, emp.Lastname)"><img src="../icon/pencil-square.svg"></button></td>
                        </tr>
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

        <div v-if="EmployeeEdit" class="row">
            <div id="employeeEdit" class="col">
                <div>
                    Firstname: <input type="text" v-model="employeeEditInfo.firstName" /><br />
                    Lastname: <input type="text" v-model="employeeEditInfo.lastName" />
                    <form @submit.prevent="changeEmployeeInformationPost">
                        <button type="submit" class="btn btn-outline-info btn-sm">Submit changes.</button>
                    </form>
                </div>
            </div>
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
                employeeEditInfo: {
                    id: 0,
                    firstName: "",
                    lastName: ""
                },
                emp: {
                    name: "",
                    empid: ""
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
            // Hämtar alla anställda och retunerar de
            fetchEmployee(){
                this.EmployeeGet = null;
                this.Employeeloading = true;
                this.EmployeeEdit = false;

                fetch('Employee')
                    .then(r => r.json())
                    .then(json => {
                        this.EmployeeGet = json;
                        this.Employeeloading = false;
                        return;
                    })
                    .catch(err => {
                        err
                    });;
            },
            // Ändrar om en anstäld är tillgänglig för ett request.
            changeStatusEmployee(employee, isAvailable) {
                this.EmployeePost = null;
                //this.EmployeePostLoading = true;

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
                        //this.EmployeePostLoading = false;
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
                //this.EmployeePostLoading = true;

                fetch('Employee/Roll', {
                })
                .then(r => r.json())
                .then(json => {
                    this.EmployeeRoll = json;
                    this.fetchEmployee();
                    //this.EmployeePostLoading = false;
                    return;
                })
                .catch(err => {
                    err
                });
            },
            // Tar fram vyn för att ändra information 
            changeEmployeeInformation(id, firstName, lastName) {
                this.EmployeeGet = null;
                this.EmployeePost = null;
                this.EmployeeRoll = null;
                this.EmployeeEdit = true;

                this.employeeEditInfo.id = id;
                this.employeeEditInfo.firstName = firstName;
                this.employeeEditInfo.lastName = lastName;
                console.log(this.employeeEditInfo);
            },
            // Tar fram vyn för att ändra information 
            changeEmployeeInformationPost() {
                console.log(this.employeeEditInfo);
                fetch('Employee/ChangeInformation/', {
                    method: 'POST',
                    headers: {
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify({ id: this.employeeEditInfo.id, firstName: this.employeeEditInfo.firstName, lastName: this.employeeEditInfo.lastName })
                })
                    .then(r => r.json())
                    .then(json => {
                        //this.EmployeePost = json;
                        this.fetchEmployee();
                        //this.EmployeePostLoading = false;
                        return;
                    })
                    .catch(err => {
                        err
                    });
                
            }
        },
    });

</script>

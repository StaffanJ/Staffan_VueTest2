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
                            <th>Name</th>
                            <th>Number of times</th>
                            <th>Is available for requests</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="emp in EmployeeGet" :key="emp.id">
                            <td><img src="../icon/person-square.svg"></td>
                            <!--<td>{{ emp.id }}</td>-->
                            <td>{{ emp.name }}</td>
                            <td>{{ emp.numOfTimes }}</td>
                            <td v-if="emp.isAvailable === 0" id="employeeStatusHover">
                                <button type="submit" class="btn" @click.prevent="changeStatusEmployee(emp.id, emp.isAvailable)"><img src="../icon/exclamation-circle.svg"></button>
                            </td>
                            <td v-if="emp.isAvailable === 1" id="employeeStatusHover">
                                <button type="submit" class="btn" @click.prevent="changeStatusEmployee(emp.id, emp.isAvailable)"><img src="../icon/exclamation-circle-fill.svg"></button>
                            </td>
                            
                        </tr>
                    </tbody>
                </table>
            </div>
            <div v-if="EmployeeRoll" class="col" id="employeesPost">
                <table class="table table-bordered caption-top">
                    <caption>Employee for the next task</caption>
                    <thead class="table-dark">
                        <tr>
                            <th></th>
                            <!--<th>Id</th>-->
                            <th>Name</th>
                            <th>Number of times</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-if="EmployeeRoll">
                            <td><img src="../icon/person-workspace.svg" /></td>
                            <!--<td>{{ EmployeeRoll.id }}</td>-->
                            <td>{{ EmployeeRoll.name }}</td>
                            <td>{{ EmployeeRoll.numOfTimes }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>

        </div>

        <div class="row">
            <div id="employeeRoll" class="col">
                <div>
                    <form @submit.prevent="rollEmployee">
                        <button type="submit" class="btn btn-outline-info btn-sm">Who gets to be the "lucky" one?</button>
                    </form>
                </div>
            </div>
        </div>
    </div>

</template>

<script lang="js">
    import { defineComponent } from 'vue';

    export default defineComponent({
        data() {
            return {
                EmployeeGet: null,
                EmployeePost: null,
                EmployeeRoll: null,
                //employee: [],
                emp: {
                    name: "",
                    empid: ""
                }
            };
        },
        async created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchEmployee();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchEmployee'
        },
        methods: {
            fetchEmployee(){
                this.EmployeeGet = null;
                this.Employeeloading = true;

                fetch('Employee')
                    .then(r => r.json())
                    .then(json => {
                        this.EmployeeGet = json;
                        this.Employeeloading = false;
                        return;
                    });
            },
            changeStatusEmployee(employee, isAvailable) {
                console.log(employee);
                this.EmployeePost = null;
                //this.EmployeePostLoading = true;

                fetch('Employee/ChangeStatus/', {
                    method: 'POST',
                    headers: {
                        'Content-type': 'application/json'
                    },
                        //body: JSON.stringify({ data: employee })
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
            rollEmployee() {
                this.EmployeeRoll = null;
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
            }
        },
    });

</script>

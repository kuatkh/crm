export const allServices = {
	// addStudent,
	// viewStudents,
	addCurrentUser,
}

// function addStudent(student) {
//  if (student) {
//    var students = JSON.parse(localStorage.getItem('students'));
//    if(students === null) {
//     students = [];
//    }
//    students.push(student);
//    localStorage.setItem('students', JSON.stringify(students));
//    return student;
//  }
//  return null;
// }

// function viewStudents() {
//  var students = JSON.parse(localStorage.getItem('students'));
//  return students === null ? [] : students;
// }

function addCurrentUser(userData) {
	if (userData) {
		localStorage.setItem('currentUser', JSON.stringify(userData))
		return userData
	}
	return null
}

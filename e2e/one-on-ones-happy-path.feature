Feature: One on Ones
# karate docs https://github.com/karatelabs/karate?tab=readme-ov-file#karate-fork

Background:
* header Content-Type = 'application/json'

Scenario: Happy Path

    * def jsUtils = read('./js-utils.js')
    * def authApiRootUrl = jsUtils().getEnvVariable('AUTH_API_ROOT_URL')
    * def employeesApiRootUrl = jsUtils().getEnvVariable('EMPLOYEES_API_ROOT_URL')
    * def apiRootUrl = jsUtils().getEnvVariable('API_ROOT_URL')
    * def authLogin = jsUtils().getEnvVariable('AUTH_FIRST_TENANT_LOGIN_WITH_ALL_PERMISSIONS')
    * def authPassword = jsUtils().getEnvVariable('AUTH_FIRST_TENANT_PASSWORD_WITH_ALL_PERMISSIONS')

    # Authentication
    Given url authApiRootUrl
    And path '/auth/login'
    And request
    """
    {
        "login": "#(authLogin)",
        "password": "#(authPassword)"
    }
    """
    And method POST
    Then status 200

    * def accessToken = karate.toMap(response.accessToken.value)
    * def decodedAccessToken = jsUtils().getDecodedToken(accessToken)

    * configure headers = jsUtils().getAuthHeaders(accessToken)
    
    # Step 0: Get all employees and pick any of them as a mentee
    Given url employeesApiRootUrl
    Given path 'internal/get-employees'
    When method GET

    # We don't care that much who it is, let it be the first one
    * def menteeEmployeeId = response[0].employeeId
    * def menteeEmployeeFullName = response[0].fullName

    * def mentorEmployeeId = Number(decodedAccessToken.employeeId)
    * def mentorEmployeeFullName = response.find(x => x.employeeId == mentorEmployeeId).fullName
    
    # Step 1: Create a new one on one
    * def oneOnOneRandomNote = '[API-E2E]-Test-one-on-one-' + Math.random()
    
    Given url apiRootUrl
    Given path 'one-on-ones'
    And request
    """
    {
        "menteeEmployeeId": "#(menteeEmployeeId)",
        "date": "2025-09-03",
        "note": "#(oneOnOneRandomNote)",
    }
    """
    When method POST
    Then status 200

    * def newOneOnOneId = response.newOneOnOneId

    # Step 2: Verify that one on one is in the list with the id and generated note
    Given url apiRootUrl
    Given path 'one-on-ones'
    And param menteeEmployeeId = menteeEmployeeId
    When method GET
    And match response.oneOnOnes contains
    """
    {
        "id": "#(newOneOnOneId)",
        "date": "2025-09-03",
        "note": "#(oneOnOneRandomNote)",
        "mentor": {
            "employeeId": "#(mentorEmployeeId)",
            "fullName": "#(mentorEmployeeFullName)",
        },
    }
    """
    And assert response.mentee.employeeId == menteeEmployeeId
    And assert response.mentee.fullName == "N/A"

    # Cleanup: Delete the one on one (hard delete)
    Given path 'one-on-ones', newOneOnOneId, 'hard-delete'
    When method DELETE
    Then status 200
    And match response == { isDeleted: true }

    # Cleanup Verification: Verify that one on one was deleted
    Given url apiRootUrl
    Given path 'one-on-ones'
    When method GET
    And assert response.oneOnOnes.filter(x => x.id == newOneOnOneId).length == 0

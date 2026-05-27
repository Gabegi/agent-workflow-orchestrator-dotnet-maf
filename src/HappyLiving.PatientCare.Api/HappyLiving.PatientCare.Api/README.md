# HappyLiving.PatientCare.Api

Minimal API serving live patient data. Used as the data source for the MCP agent in the agent workflow orchestrator.

---

## Endpoints

### Medications Due
```
GET /medications-due/{patientId}
GET /medications-due/{patientId}/now
GET /medications-due/ward/{wardId}
```

### Care Plans
```
GET /careplans/{patientId}
GET /careplans/{patientId}/goals
GET /careplans/{patientId}/interventions
```

### Allergies
```
GET /allergies/{patientId}
GET /allergies/{patientId}/severe
```

### Appointments
```
GET /appointments/{patientId}
GET /appointments/{patientId}/upcoming
```

### History
```
GET /history/{patientId}
GET /history/{patientId}/recent
```

---

## Test Data IDs

| Type | IDs |
|---|---|
| Patients | `P001`, `P002`, `P003`, `P004`, `P005` |
| Wards | `W001`, `W002`, `W003` |

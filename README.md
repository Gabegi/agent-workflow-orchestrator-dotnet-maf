# Agent Workflow Orchestrator — .NET / Azure AI Foundry

A multi-agent orchestration system built with the Microsoft Agent Framework (.NET) on Azure AI Foundry. Designed for clinical environments where nurses need fast, reliable answers from multiple data sources.

---

## Overview

A nurse asks a question in chat. The orchestrator classifies the intent and routes it to the right specialist agent — or fans out to multiple agents concurrently when the question spans both procedural knowledge and patient-specific data. Results come back with source citations and confidence scores, and the orchestrator synthesizes a final structured response.

```
Nurse asks question
       │
       ▼
  Orchestrator
  (intent detection + routing)
       │
       ├──────────────────────┬──────────────────────┐
       ▼                      ▼                      ▼
  O365 Agent            SQL Agent           AI Search Agent
  (SharePoint /         (patient DB /       (Azure AI Search /
   Graph API)            SQL)                vector RAG)
       │                      │                      │
       └──────────────────────┴──────────────────────┘
                              │
                              ▼
                  Synthesized structured response
                  (with citations + confidence scores)
```

---

## Routing Logic

| Question type | Agent(s) invoked |
|---|---|
| Protocol / procedure | O365 Agent |
| Patient medication history | SQL Agent |
| Combined | O365 Agent + SQL Agent (concurrent) |
| Document / indexed knowledge | AI Search Agent |

---

## Agents

### O365 Agent
Queries SharePoint and Office 365 content via the **Microsoft Graph API** — the gateway to all O365 data. Used for clinical protocols, procedures, and policy documents stored in SharePoint.

### SQL Agent
Queries the patient database directly via SQL. Used for patient-specific data such as medication history, dosage records, and administration logs.

### Azure AI Search Agent
Queries an **Azure AI Search** index using vector search (RAG). Clinical protocols and documents are indexed here and retrieved by semantic similarity. This is the primary RAG agent for unstructured document retrieval.

---

## Infrastructure

Provisioned via Terraform on Azure:

- Azure AI Foundry Hub + Project
- Azure Key Vault
- Azure Storage Account
- Azure Resource Group

CI/CD via GitHub Actions (self-hosted runner, MSI authentication):

- `1-deploy-infrastructure.yml` — provisions all Azure resources
- `4-destroy-infrastructure.yml` — tears down all infrastructure

---

## Tech Stack

- .NET (C#)
- Microsoft Agent Framework (MAF)
- Azure AI Foundry
- Azure AI Search
- Microsoft Graph API
- Terraform
- GitHub Actions ()

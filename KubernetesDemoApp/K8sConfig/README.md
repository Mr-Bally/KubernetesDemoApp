# Local cluster Setup

kubectl create namespace demo-app

## Create service account so you aren't using the default service account
kubectl apply -f serviceAccount.yml


## Create service account secret
kubectl apply -f serviceAccountSecret.yml


## Add admin role
kubectl create rolebinding demo-app-admin --clusterrole=cluster-admin --serviceaccount=default:demo-app-service-account --namespace=demo-app


## Get service account secret (api key) for local kube config
kubectl describe secrets/demo-app-service-account-secret



# Cotação Analyzer

Sistema de análise e ranqueamento inteligente de cotações, com foco em flexibilidade, clareza e performance.  
Desenvolvido em **.NET 8**, arquitetura em camadas, e frontend em **React/TypeScript**.

---

## Problema que resolvemos

Empresas que recebem múltiplas propostas para compra de produtos geralmente analisam essas cotações de forma manual — considerando apenas o menor preço. Isso ignora fatores essenciais como **prazo de entrega** e **frete incluso**.

---

## Solução

O *Cotação Analyzer* permite:
- ✅ Cadastro de produtos
- ✅ Cadastro de cotações (com múltiplos itens)
- ✅ Definição de **pesos personalizados** para:
  - Valor proposto
  - Frete incluso
  - Prazo de entrega
- ✅ Ranqueamento automático entre múltiplas cotações com base nesses pesos
- ✅ API REST documentada com Swagger
- ✅ Integração com frontend (React em progresso)

---

## Lógica de ranqueamento

1. O usuário define os **pesos** (ex: valor = 3, prazo = 2, frete = 1).
2. O sistema calcula **score normalizado (0 a 1)** para cada critério.
3. Apenas cotações com **produtos em comum** são comparadas entre si.
4. O resultado traz todas as cotações, mas ranqueia somente aquelas comparáveis.

---

## Arquitetura

- Backend: **.NET Core 8.0**
- ORM: **Entity Framework Core**
- Frontend: **React**
- Mapeamento: **AutoMapper**
- Validação: manual (com exceções customizadas)
- Testes: xUnit + Moq (em progresso)

---

## Como rodar localmente

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/cotacao-analyzer.git
cd cotacao-analyzer

# Aplique as migrations (garanta que o PostgreSQL está rodando)
dotnet ef database update

# Rode a API
dotnet run --project .\CotacaoAnalyzer\

# Rode o frontend 
cd .\CotacaoAnalyzer.Web\ClientApp\  
npm install
npm start
